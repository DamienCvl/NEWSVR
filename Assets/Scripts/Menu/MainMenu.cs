using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.UI;
using System;
using MySql.Data.MySqlClient;
using System.IO;
using Assets.Scripts.Core;
using System.Linq;

public class MainMenu : MonoBehaviour
{
   
    public Button playGameButton;
    public Button profilButton;

    public Text sortedByTxt;
    public Text newsPrompt;
    public Text state;

    //notification list
    public GameObject notifTemplate;
    public GameObject content;

    public const int MAX_NOTIF_TO_DISPLAY = 20;

  


    public List<GameObject> listBtnNews = new List<GameObject>();


    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Start()
    {
        Database.GetTagColors();
        if (StaticClass.CurrentPlayerName != "")
        {
            state.text = "User log :  " + StaticClass.CurrentPlayerName;
            profilButton.interactable = (true);
            playGameButton.interactable = (true);
            StaticClass.newsList.Clear();
            Database.GenerateNewsList();
            NotifSortedByDate();
        }
        else
        {
            profilButton.interactable = (false);
            playGameButton.interactable = (false);
        }
    }

    /*******************************/
    /****** Notification List ******/
    /*******************************/
    public void DisplayNews(List<News> ln) { 

        foreach(News n in ln)
        {
            var copy = Instantiate(notifTemplate);
            copy.transform.parent = content.transform;
            copy.transform.GetComponentInChildren<Text>().text = n.GetTitle() + "\n" + n.GetTagsToString() + " - " + n.GetDist() + "m - ("+n.nbOfView+"-"+n.nbComment+").";
            copy.SetActive(true);

            if (StaticClass.newsBeaconedList.Exists(x => x == n.GetId()))
            {
                ColorBlock cb = copy.GetComponent<Button>().colors;
                cb.normalColor = n.GetNewsColor();
                copy.GetComponent<Button>().colors = cb; 
            }


           

            copy.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    if (StaticClass.newsBeaconedList.Exists(x => x == n.GetId()))
                    {
                        StaticClass.newsBeaconedList.Remove(n.GetId());
                        copy.GetComponent<Button>().colors = notifTemplate.GetComponent<Button>().colors;
                        
                    }
                    else
                    {
                        StaticClass.newsBeaconedList.Add(n.GetId());

                        ColorBlock cb = copy.GetComponent<Button>().colors;
                        cb.normalColor = n.GetNewsColor();
                        copy.GetComponent<Button>().colors = cb;
                    }

                    //TODO ugly way to refresh the content
                    content.SetActive(false);
                    content.SetActive(true);
                }
            );

            listBtnNews.Add(copy);
        }
    }

/**************************/
/****** MENU BUTTONS ******/
/**************************/
public void GoToRegister()
    {
        SceneManager.LoadScene(3);
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene(4);
    }

    public void GoToSetting()
    {
        SceneManager.LoadScene(5);
    }

    public void GoToDevMode()
    {
        SceneManager.LoadScene(6);
    }

    public void Play()
    {
        StaticClass.nbrCommentDisplayed = Database.ReadNbrCommentDisplayed();
        StaticClass.CommentPosition = (CommentGameObject.Positions)Database.ReadCommentPosition();
        SceneManager.LoadScene(1);
    }

    // Should load the VR device to go from dev mode to town simulation but doesn't seem to work
    // Once you have gone to devmode you need to quit the application to not cause bug when going to Town simulation.
    IEnumerator LoadDevice(string newDevice, bool enable)
    {
        XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        XRSettings.enabled = enable;
    }

    void EnableVR()
    {
        StartCoroutine(LoadDevice("OpenVR", true));
    }

    void DisableVR()
    {
        StartCoroutine(LoadDevice("", false));
    }


    /**************************************/
    /****** NOTIFICATION LIST ACTION ******/
    /**************************************/

    //Date sort action
    public void NotifSortedByDate()
    {
        ClearNotification();
        List<News> SortedList = StaticClass.newsList.OrderBy(o => o.GetDate()).ToList();
        DisplayNews(SortedList);
        sortedByTxt.text = "The 20 recent news";
    }

    //Closest sort action
    public void NotifSortedByDist()
    {
        ClearNotification();
        List<News> SortedList = StaticClass.newsList.OrderBy(o => o.GetDist()).ToList();
        DisplayNews(SortedList);
        sortedByTxt.text = "The 20 closest news";
    }
    //Popularity sort action
    public void NotifSortedByPoularity()
    {
        ClearNotification();
        List<News> SortedList = StaticClass.newsList.OrderByDescending(o => o.nbOfView).ToList();
        DisplayNews(SortedList);
        sortedByTxt.text = "The 20 most-viewed news";
    }
    //Tag sort action
    public void NotifSortedByTag()
    {
        ClearNotification();
        DisplayNews(StaticClass.newsList);
        sortedByTxt.text = "Sorted by tags";
    }

    //Clear the notification list
    public void ClearNotification()
    {
        if(listBtnNews.Count > 0)
        {
            foreach (GameObject go in listBtnNews)
            {
                Destroy(go);
            }
            listBtnNews.Clear();
            Debug.Log(StaticClass.newsBeaconedList.Count);
           // StaticClass.newsBeaconedList.Clear();
        }
    }
}