using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Windows.Speech;
using System.Linq;

using UnityEngine.UI;


namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class MicroComments : MonoBehaviour
    {
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand;

        [Tooltip("Name of the attachment transform under in the hand's hierarchy which the object should should snap to.")]
        public string attachmentPoint;

        [Tooltip("When detaching the object, should it return to its original parent?")]
        public bool restoreOriginalParent = false;

        private bool attached = false;

        public UnityEvent onPickUp;
        public UnityEvent onDetachFromHand;

        private GameObject CommentPreFab;
        private GameObject Comments;
        public GameObject Highlight;

        private Vector3 transformInit;

        public Text titleOfNews;

        private DictationRecognizer dictationRecognizer;

        // Used to see if there is a hand to release when you go out of the news and which one it is
        private Hand handToRelease;

        private void Start()
        {
            // Usefull to use it as parents location
            Comments = GameObject.Find("Comments");
            CommentPreFab = (GameObject)Resources.Load("Prefabs/News/Comment", typeof(GameObject));

            // Create the voice recognition system
            dictationRecognizer = new DictationRecognizer();
            dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
            dictationRecognizer.DictationResult += onDictationResult;
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

        private void onDictationResult(string text, ConfidenceLevel confidence)
        {
          // Create the comment with the text said and pass the title to know wich news own the comment
          var comment = Instantiate(CommentPreFab, Comments.transform);
          comment.GetComponent<NewsComment>().FillText(text, titleOfNews.text.ToString());
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
        private void OnHandHoverBegin(Hand hand)
        {
            Highlight.SetActive(true);

            // "Catch" the throwable by holding down the interaction button instead of pressing it.
            // Only do this if it isn't attached to another hand
            if (!attached)
            {
                if (hand.GetStandardInteractionButton())
                {
                    hand.AttachObject(gameObject, attachmentFlags, attachmentPoint);
                }
            }
        }


        //-------------------------------------------------
        private void OnHandHoverEnd(Hand hand)
        {
            Highlight.SetActive(false);

            ControllerButtonHints.HideButtonHint(hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
        }


        //-------------------------------------------------
        private void HandHoverUpdate(Hand hand)
        {
            //Trigger got pressed
            if (!attached && hand.GetStandardInteractionButtonDown())
            {
                hand.AttachObject(gameObject, attachmentFlags, attachmentPoint);
                ControllerButtonHints.HideButtonHint(hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
            }
        }

        //-------------------------------------------------
        private void OnAttachedToHand(Hand hand)
        {
            attached = true;

            handToRelease = hand;

            onPickUp.Invoke();

            hand.HoverLock(null);

            dictationRecognizer.Start();
        }


        //-------------------------------------------------
        private void OnDetachedFromHand(Hand hand)
        {
            attached = false;

            handToRelease = null;

            onDetachFromHand.Invoke();

            hand.HoverUnlock(null);

            // Return the microphone to no rotation 
            transform.rotation = new Quaternion(0, 0, 0, 0);

            transform.localPosition = transformInit;

            dictationRecognizer.Stop();
        }


        //-------------------------------------------------
        private void HandAttachedUpdate(Hand hand)
        {
            //Trigger got released
            if (!hand.GetStandardInteractionButton())
            {
                // Detach ourselves late in the frame.
                // This is so that any vehicles the player is attached to
                // have a chance to finish updating themselves.
                // If we detach now, our position could be behind what it
                // will be at the end of the frame, and the object may appear
                // to teleport behind the hand when the player releases it.
                StartCoroutine(LateDetach(hand));
            }
        }


        //-------------------------------------------------
        private IEnumerator LateDetach(Hand hand)
        {
            yield return new WaitForEndOfFrame();

            hand.DetachObject(gameObject, restoreOriginalParent);
        }


        //-------------------------------------------------
        private void OnHandFocusAcquired(Hand hand)
        {
            gameObject.SetActive(true);
        }


        //-------------------------------------------------
        private void OnHandFocusLost(Hand hand)
        {
            gameObject.SetActive(false);
        }
    }
}
