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
        //DisplayNews();
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
            copy.transform.GetComponentInChildren<Text>().text = n.GetTitle() + "\n" + n.GetTagsToString() + " - " + n.GetDist() + "m - ("+n.GetViews()+"-"+n.GetNbComment()+").";
            copy.SetActive(true);


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





//TODO: Add a parameter in order to know how to sort
/* public void DisplayNews()
 {

     int index;
     int nbTotalNews = StaticClass.newsList.Count;

     if (nbTotalNews >= 10)
     {
         index = 10;
     }
     else
     {
         index = nbTotalNews;
     }

     for(int i=0 ; i < index; i++)
     {
         News n = StaticClass.newsList[i];
         listBtnNews[i].gameObject.SetActive(true);
         listBtnNews[i].GetComponentInChildren<Text>().text = n.GetTitle() + "\n" + n.GetTagsToString() + " - " + n.GetDist() + "m .";

     }


 }





 public void OnClickNewsActivateBeacon(Button temp)
 {

     int index = listBtnNews.FindIndex(a => a == temp);
     News n = StaticClass.newsList[index]; ///changer pour mettre la liste [10] de´s news "active" (en place actuellement)

     if( StaticClass.newsBeaconedList.Exists(x => x == n.GetId()) )
     {

         StaticClass.newsBeaconedList.Remove(n.GetId());
         temp.colors = ColorBlock.defaultColorBlock;
     }
     else
     {
         StaticClass.newsBeaconedList.Add(n.GetId());
         Color color = StaticClass.tagPrefColorList[n.GetTags()[0]];  // we take the choosen color (by the player) of the "main" (first) tag of the news

         ColorBlock cb = temp.colors;
         cb.normalColor = color;
         temp.colors = cb;
     }



 }*/


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
        // Disable VR to go to the dev mode since it's on a screen.
        DisableVR();
        SceneManager.LoadScene("DevMode", LoadSceneMode.Single);
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



    public void NotifSortedByDate()
    {
        ClearNotification();
        DisplayNews(StaticClass.newsList); 
    }
    
    //Closest
    public void NotifSortedByDist()
    {
        ClearNotification();
        List<News> SortedList = StaticClass.newsList.OrderBy(o => o.GetDist()).ToList();
        DisplayNews(SortedList);
    }

    public void NotifSortedByPoularity()
    {
        ClearNotification();
        List<News> SortedList = StaticClass.newsList.OrderByDescending(o => o.GetViews()).ToList();
        DisplayNews(SortedList);
    }

    public void NotifSortedByTag()
    {

    }

    public void ClearNotification()
    {
        if(listBtnNews.Count > 0)
        {
            foreach (GameObject go in listBtnNews)
            {
                Destroy(go);
            }
            listBtnNews.Clear();
            StaticClass.newsBeaconedList.Clear();
        }
    }
}