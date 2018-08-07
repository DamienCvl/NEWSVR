using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevModeNewsInstantiate : MonoBehaviour {

    public Text Information;
    public Button DeleteButton;
    private string title;

    private GameObject Verif;

    private void Start()
    {
        Verif = GameObject.Find("VerifNews");
        DeleteButton.onClick.AddListener(DeleteAction);
    }

    public void FillTitle(string titleGiven, string fullInformation)
    {
        title = titleGiven;
        Information.text = fullInformation;
    }

    public void ChangePosition(Vector3 newPos)
    {
        gameObject.transform.localPosition = newPos;
    }

    private void DeleteAction()
    {
        Verif.GetComponent<DevModeVerifNews>().SetNameNews(title, gameObject);
    }
}
