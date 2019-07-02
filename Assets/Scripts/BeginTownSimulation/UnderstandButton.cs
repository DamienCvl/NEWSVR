using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Valve.VR;

/*
 * Handle the Button at the beginning.
 * At the start it's like you're in a news so when you "take" the understand button do the same actions as when you leave a news.
 */

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class UnderstandButton : Grabbable
    {
        private GameObject Teleport;
        public SteamVR_Action_Boolean UI_Interaction_Action = SteamVR_Input.GetBooleanAction("InteractUI");
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
        private new void OnDetachedFromHand(Hand hand)
        {
            UnderstandAction();
        }
    }
}
