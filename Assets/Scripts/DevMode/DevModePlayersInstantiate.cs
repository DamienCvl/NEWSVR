using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Used to instantiate each rows of the player view
 */
namespace Assets.Scripts.DevMode
{
    public class DevModePlayersInstantiate : MonoBehaviour
    {

        public Text Information;
        public Button DeleteButton;
        private string namePlayer;

        private GameObject Verif;

        private void Start()
        {
            Verif = GameObject.Find("VerifPlayers");
            DeleteButton.onClick.AddListener(DeleteAction);
        }

        public void FillName(string NameGiven, string fullInformation)
        {
            namePlayer = NameGiven;
            Information.text = fullInformation;
        }

        public void ChangePosition(Vector3 newPos)
        {
            gameObject.transform.localPosition = newPos;
        }

        private void DeleteAction()
        {
            Verif.GetComponent<DevModeVerifPlayers>().SetNamePlayer(namePlayer, gameObject);
        }
    }
}