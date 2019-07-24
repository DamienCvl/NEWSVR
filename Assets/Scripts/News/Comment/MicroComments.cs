using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Windows.Speech;
using System.Linq;

using UnityEngine.UI;

/*
 * This handles the microphone.
 * The DictationRecognizer system is here and you use it to create comment with the prefab of comment.
 * The DictationRecognizer system will start when you take the microphone in your hand.
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class MicroComments : Grabbable
    {

        private GameObject CommentPreFab;
        private GameObject Player;

        private Vector3 transformInit;

        private DictationRecognizer dictationRecognizer;

        // Used to see if there is a hand to release when you go out of the news and which one it is
        private Hand handToRelease;

        private void Start()
        {
            // Usefull to use it as parents location
            Player = GameObject.FindObjectOfType<Player>().gameObject;
            CommentPreFab = (GameObject)Resources.Load("Prefabs/News/Comment", typeof(GameObject));

            // Create the voice recognition system
            dictationRecognizer = new DictationRecognizer();
            dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
            dictationRecognizer.DictationResult += OnDictationResult;
        }

        private void OnEnable()
        {
            transformInit = transform.localPosition;
        }

        private void OnDisable()
        {
            if (handToRelease != null)
            {
                OnDetachedFromHand(handToRelease);
            }

        }

        private void OnDictationResult(string text, ConfidenceLevel confidence)
        {
            // Create the comment with the text said
            var comment = Instantiate(CommentPreFab, Player.transform);
            comment.GetComponent<CommentGameObject>().FillText(text);

            // Put the new comment in front of you
            comment.transform.position += Player.transform.forward * 0.4f;
            comment.transform.rotation = Quaternion.LookRotation(Player.transform.position - comment.transform.position, Vector3.up);
        }

        // Use if there is a problem in the voice recognition
        private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
        {
            if (cause != DictationCompletionCause.Complete)
            {
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", cause);
            }
        }

        //-------------------------------------------------
        protected new void OnAttachedToHand(Hand hand)
        {
            base.OnAttachedToHand(hand);
            handToRelease = hand;
            dictationRecognizer.Start();
        }


        //-------------------------------------------------
        protected new void OnDetachedFromHand(Hand hand)
        {
            base.OnDetachedFromHand(hand);
            handToRelease = null;
            // Return the microphone to no rotation 
            transform.rotation = new Quaternion(0, 0, 0, 0);

            transform.localPosition = transformInit;

            dictationRecognizer.Stop();
        }
    }
}
