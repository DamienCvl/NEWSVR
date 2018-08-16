using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

/*
 * Handle the Button at the beginning.
 * At the start it's like you're in a news so when you "take" the understand button do the same actions as when you leave a news.
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class UnderstanButton : MonoBehaviour
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
        private GameObject Teleport;

        public GameObject IndicationBegin;
        public GameObject EveryNews;

        private void Start()
        {
            Teleport = GameObject.Find("TeleportController");
        }

        // Action when you click the button, delete all the begin Information
        private void UnderstandAction()
        {
            var tpcontrol = Teleport.GetComponent<TeleportController>();
            tpcontrol.changeTeleport(true);

            EveryNews.GetComponent<NewsPlacement>().aNewsIsOpen = false;

            Destroy(IndicationBegin);
            // Wait 1 second to delete the IndicationBegin button to not cause bug
            Destroy(gameObject, 1);
        }

        //-------------------------------------------------
        private void OnHandHoverBegin(Hand hand)
        {
            Highlight.SetActive(true);

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

            onPickUp.Invoke();

            hand.HoverLock(null);
        }


        //-------------------------------------------------
        private void OnDetachedFromHand(Hand hand)
        {
            attached = false;

            onDetachFromHand.Invoke();

            hand.HoverUnlock(null);

            UnderstandAction();
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
