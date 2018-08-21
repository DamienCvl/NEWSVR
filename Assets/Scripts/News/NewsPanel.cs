using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * This handle the article when you open a news item.
 * The microphone and the reaction are attached to this, so you change it's position, it moves everything.
 * You can find the srcoll function here too.
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class NewsPanel : MonoBehaviour
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

        private GameObject HeadCollider;
        public GameObject Highlight;

        public GameObject ContentNews;
        private RectTransform numberToMove;
        private float whereIsTheArticle;

        // Used to see if there is a hand to release when you go out of the news and which one it is
        private Hand handToRelease;

        private void Start()
        {
            whereIsTheArticle = 0.0f;
            HeadCollider = GameObject.Find("HeadCollider");

            // Put the news in front of the player the first time the player pick up the newsSphere
            // Change the second parameter of transform.position to change the heigth of the news item, microphone and reaction box.
            transform.position = new Vector3(HeadCollider.transform.position.x, 1.1f, HeadCollider.transform.position.z);
            transform.rotation = new Quaternion(0, HeadCollider.transform.rotation.y, 0, HeadCollider.transform.rotation.w);
            transform.Translate(new Vector3(HeadCollider.transform.forward.x, 0.0f, HeadCollider.transform.forward.z), Space.World);
        }

        private void OnEnable()
        {
            Highlight.SetActive(false);

            // Put the news in front of the player every time the player pick up the newsSphere
            transform.position = new Vector3(HeadCollider.transform.position.x, 1.1f, HeadCollider.transform.position.z);
            transform.rotation = new Quaternion(0, HeadCollider.transform.rotation.y , 0, HeadCollider.transform.rotation.w);
            transform.Translate(new Vector3(HeadCollider.transform.forward.x, 0.0f, HeadCollider.transform.forward.z), Space.World);

        }

        private void OnDisable()
        {

            if(handToRelease != null)
            {
                OnDetachedFromHand(handToRelease);
            }

        }

        //-------------------------------------------------
        private void OnHandHoverBegin(Hand hand)
        {
            // Put the highlight panel around the news.
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
            // Put away the highlight panel 
            Highlight.SetActive(false);
            ControllerButtonHints.HideButtonHint(hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
        }


        //-------------------------------------------------
        private void HandHoverUpdate(Hand hand)
        {
            ScrollArticle(hand);

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

        // Function to scroll the article
        private void ScrollArticle(Hand hand)
        {
            var device = hand.controller;

            // The component to change to make the article scroll
            RectTransform numberToMove = (RectTransform)ContentNews.GetComponent(typeof(RectTransform));

            // Find how to get the position of the thumb on the touchpad
            if (device != null)
            {
                if (device.GetAxis().y < 0)
                {
                    // Move the article from the top to the bottom by step
                    whereIsTheArticle = whereIsTheArticle + 0.003f;
                    numberToMove.offsetMax = new Vector2(numberToMove.offsetMax.x, whereIsTheArticle);
                }

                if (device.GetAxis().y > 0)
                {
                    if (whereIsTheArticle >= 0.0f)
                    {
                        // Move the article from the bottom to the top by step
                        whereIsTheArticle = whereIsTheArticle - 0.003f;
                        numberToMove.offsetMax = new Vector2(numberToMove.offsetMax.x, whereIsTheArticle);
                    }
                }
            }
        }
    }
}
