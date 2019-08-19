using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Used to instantiate each rows of the comment view
 */
namespace Assets.Scripts.DevMode
{
    public class DevModeCommentsInstantiate : MonoBehaviour
    {

        public Text Information;
        public Button DeleteButton;
        private int id;

        private GameObject Verif;

        private void Start()
        {
            Verif = GameObject.Find("VerifComments");
            DeleteButton.onClick.AddListener(DeleteAction);
        }

        public void FillText(int idGiven, string fullInformation)
        {
            id = idGiven;
            Information.text = fullInformation;
        }

        public void ChangePosition(Vector3 newPos)
        {
            gameObject.transform.localPosition = newPos;
        }

        private void DeleteAction()
        {
            Verif.GetComponent<DevModeVerifComments>().SetIdComments(id, gameObject);
        }
    }
}
