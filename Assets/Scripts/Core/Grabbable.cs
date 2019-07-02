using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Valve.VR;

/*
 * Allow the object to be grab
 */

namespace Valve.VR.InteractionSystem
{

    [RequireComponent(typeof(Interactable))]
    public class Grabbable : MonoBehaviour
    {

        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand;

        [Tooltip("When detaching the object, should it return to its original parent?")]
        public bool restoreOriginalParent = false;

        protected bool attached = false;

        protected Interactable interactable;

        //-------------------------------------------------
        void Awake()
        {
            interactable = this.GetComponent<Interactable>();
        }

        //-------------------------------------------------
        protected void OnHandHoverBegin(Hand hand)
        {
        }


        //-------------------------------------------------
        protected void OnHandHoverEnd(Hand hand)
        {
        }


        //-------------------------------------------------
        protected void HandHoverUpdate(Hand hand)
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();
            bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

            if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
            {
                // Call this to continue receiving HandHoverUpdate messages,
                // and prevent the hand from hovering over anything else
                hand.HoverLock(interactable);

                // Attach this object to the hand
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
            }
            else if (isGrabEnding)
            {
                // Detach this object from the hand
                hand.DetachObject(gameObject);

                // Call this to undo HoverLock
                hand.HoverUnlock(interactable);
            }

        }

        //-------------------------------------------------
        protected void OnAttachedToHand(Hand hand)
        {
        }


        //-------------------------------------------------
        protected void OnDetachedFromHand(Hand hand)
        {
        }


        //-------------------------------------------------
        protected void HandAttachedUpdate(Hand hand)
        {
        }

        //-------------------------------------------------
        protected void OnHandFocusAcquired(Hand hand)
        {
        }


        //-------------------------------------------------
        protected void OnHandFocusLost(Hand hand)
        {
        }
    }
}
