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
    public class UnderstandButton : MonoBehaviour
    {
        private GameObject Teleport;
        public GameObject IndicationBegin;
        public GameObject EveryNews;

        private void Start()
        {
            Teleport = GameObject.Find("TeleportController");
        }

        // Action when you click the button, delete all the begin Information
        public void UnderstandAction()
        {
            var tpcontrol = Teleport.GetComponent<TeleportController>();
            tpcontrol.changeTeleport(true);

            EveryNews.GetComponent<NewsPlacement>().aNewsIsOpen = false;

            Destroy(IndicationBegin);
            // Wait 1 second to delete the IndicationBegin button to not cause bug
            Destroy(gameObject, 1);
        }
    }
}
