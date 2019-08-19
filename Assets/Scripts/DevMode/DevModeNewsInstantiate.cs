using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Used to instantiate each rows of the news view
 */
namespace Assets.Scripts.DevMode
{
    public class DevModeNewsInstantiate : MonoBehaviour
    {

        public Text Information;
        public Button DeleteButton;
        private string title;
        private int id;

        private GameObject Verif;

        private void Start()
        {
            Verif = GameObject.Find("VerifNews");
            DeleteButton.onClick.AddListener(DeleteAction);
        }

        public void FillTitle(int idGiven, string titleGiven, string fullInformation)
        {
            id = idGiven;
            title = titleGiven;
            Information.text = fullInformation;
        }

        public void ChangePosition(Vector3 newPos)
        {
            gameObject.transform.localPosition = newPos;
        }

        private void DeleteAction()
        {
            Verif.GetComponent<DevModeVerifNews>().SetNameNews(id, title, gameObject);
        }
    }
}