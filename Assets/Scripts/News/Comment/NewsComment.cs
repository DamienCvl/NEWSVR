using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

/*
 * This handles the placement of the comment at it's creation.
 * Uses MicroComments to fill the text of the comment.
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class NewsComment : MonoBehaviour
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

        public GameObject Highlight;
        public GameObject content;
        public GameObject Buttons;
        private GameObject InTheNews;

        public TextMesh Author;
        [HideInInspector]
        public int id;
        public GameObject DeleteButton;

        [HideInInspector]
        public string titleOfNews;

        [HideInInspector]
        public string textOfComment;

        private GameObject HeadCollider;

        // Used to see if there is a hand to release when you go out of the news and which one it is
        private Hand handToRelease;

        private void Start()
        {
            HeadCollider = GameObject.Find("HeadCollider");
            InTheNews = GameObject.Find("InTheNews");

            // Put the comments around the player the first time the player pick up the newsSphere (Doesn't call OnEnable the first time)
            InTheNews.GetComponent<FloatPlacementComments>().ForCommentsPlacement = InTheNews.GetComponent<FloatPlacementComments>().ForCommentsPlacement + 1f;
            transform.position = new Vector3(HeadCollider.transform.position.x, 1.5f, HeadCollider.transform.position.z);
            transform.rotation = new Quaternion(0, HeadCollider.transform.rotation.y + InTheNews.GetComponent<FloatPlacementComments>().ForCommentsPlacement, 0, HeadCollider.transform.rotation.w);
            transform.Translate(new Vector3(this.transform.forward.x, 0.0f, this.transform.forward.z)* InTheNews.GetComponent<FloatPlacementComments>().ForCommentsPlacementDeepness, Space.World);
            InTheNews.GetComponent<FloatPlacementComments>().UpdateForCommentsPlacementDeepness();
        }


        private void OnEnable()
        {
            // Put the comments around the player every time the player pick up the newsSphere
            InTheNews.GetComponent<FloatPlacementComments>().ForCommentsPlacement = InTheNews.GetComponent<FloatPlacementComments>().ForCommentsPlacement + 0.5f;
            transform.position = new Vector3(HeadCollider.transform.position.x, 1.5f, HeadCollider.transform.position.z);
            transform.rotation = new Quaternion(0, HeadCollider.transform.rotation.y + InTheNews.GetComponent<FloatPlacementComments>().ForCommentsPlacement, 0, HeadCollider.transform.rotation.w);
            transform.Translate(new Vector3(this.transform.forward.x, 0.0f, this.transform.forward.z), Space.World);
        }

        private void OnDisable()
        {

            if (handToRelease != null)
            {
                OnDetachedFromHand(handToRelease);
            }

        }

        // Use in MicroComments script to fill the comment
        public void FillText(string text, string title)
        {
            Text textComment = content.GetComponent<Text>();
            textComment.text = text;
            titleOfNews = title;
            textOfComment = text;
            Author.text = StaticClass.CurrentPlayerName;
        }

        public void FillAuthor(string author)
        {
            Author.text = author;
        }

        // Called by validate button to destroy the buttons.
        public void DestroyButtons()
        {
            Destroy(Buttons);
        }

        //-------------------------------------------------
        private void OnHandHoverBegin(Hand hand)
        {
            Highlight.SetActive(true);

            // "Catch" the panel by holding down the interaction button instead of pressing it.
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

        }


        //-------------------------------------------------
        private void OnDetachedFromHand(Hand hand)
        {
            attached = false;

            handToRelease = null;

            onDetachFromHand.Invoke();

            hand.HoverUnlock(null);
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
