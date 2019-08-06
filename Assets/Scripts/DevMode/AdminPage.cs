﻿using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class AdminPage : MonoBehaviour
{
    //tags list
    public GameObject tagTemplate;
    public GameObject tagContent;
    public GameObject tagAddPanel;
    public GameObject tagDeletePanel;

    //news list
    public GameObject newsTemplate;
    public GameObject newsContent;

    public List<GameObject> newsListGO;
    public List<GameObject> tagListGO;

    // Start is called before the first frame update
    void Start()
    {
         DisplayNewsList();
         DisplayTagsList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddNews()
    {
        // Disable VR to go to the dev mode since it's on a screen.
        DisableVR();
        SceneManager.LoadScene("DevMode", LoadSceneMode.Single);
    }



    IEnumerator LoadDevice(string newDevice, bool enable)
    {
        XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        XRSettings.enabled = enable;
    }

    void DisableVR()
    {
        StartCoroutine(LoadDevice("", false));
    }


    public void AddTags()
    {
        tagAddPanel.SetActive(true);
    }

    public void CancelTagActionBtn()
    {
        tagAddPanel.SetActive(false);
        tagDeletePanel.SetActive(false);
    }

    public void SaveAddTag()
    {
        string newTag = tagAddPanel.GetComponentInChildren<InputField>().text;
        Debug.Log(newTag);
        if (newTag != "")
        {
            if (IsTagAlreadyExist(newTag))
            {
                tagAddPanel.GetComponentInChildren<Text>().text = "This tag already exists";
            }
            else
            {
                if (Database.InsertTag(newTag))
                {
                    tagAddPanel.GetComponentInChildren<Text>().text = newTag + " added successfully";
                    tagAddPanel.GetComponentInChildren<InputField>().text = "";    ///clear the input field
                    DisplayTagsList();  ///refresh tag list
                }
                else
                {
                    tagAddPanel.GetComponentInChildren<Text>().text = "Something wrong happened";
                }
            }  
        }
        else
        {
            tagAddPanel.GetComponentInChildren<Text>().text = "This field can't be empty";
        }
    }


    public void DisplayTagsList()
    {
        ClearList(tagListGO);
        foreach (string s in Database.GetTags())
        {
            var copy = Instantiate(tagTemplate);
            copy.transform.parent = tagContent.transform;
            copy.transform.GetComponentInChildren<Text>().text = "    " + s;
            copy.SetActive(true);
            tagListGO.Add(copy);

            copy.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    tagDeletePanel.SetActive(true);
                    tagDeletePanel.GetComponentInChildren<Text>().text = s;
                }
           );
        }
    }

    public void DisplayNewsList()
    {
        ClearList(newsListGO);
        foreach (News n in StaticClass.newsList)
        {
            var copy = Instantiate(newsTemplate);
            copy.transform.parent = newsContent.transform;
            copy.transform.GetComponentInChildren<Text>().text = "    " + n.GetId() + " - " + n.GetTitle();
            copy.SetActive(true);
            newsListGO.Add(copy);

            copy.GetComponent<Button>().onClick.AddListener(
                () =>
                {

                }
           );
        }
    }


    public void DeleteTag(Text t)
    {
        Database.RemoveTag(t.text);
        DisplayTagsList();
        tagDeletePanel.SetActive(false);
    }
    

    public bool IsTagAlreadyExist(string s)
    {
       foreach(GameObject go in tagListGO)
       {
            if(go.GetComponentInChildren<Text>().text == "    " + s)
            {
                return true;
            }
       }
       return false;
    }


    public void ClearList(List<GameObject> lgo)
    {
        if (lgo.Count > 0)
        {
            foreach (GameObject go in lgo)
            {
                Destroy(go);
            }
            lgo.Clear();
        }
    }

    public void GoBackToMenu()
    {
        StaticClass.GoBackToMenu();
    }

}