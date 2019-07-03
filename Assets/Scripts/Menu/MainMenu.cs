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

public class MainMenu : MonoBehaviour
{
   
    public Button playGameButton;
    public Button profilButton;
    

    public Text newsPrompt;
    public Text state;

    public List<Button> listBtnNews = new List<Button>(10);


    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene(0);
        }

       
    }

    private void Start()
    {
        Database.GenerateNewsList();
        DisplayNews();

        if (StaticClass.CurrentPlayerName != "")
        {
            state.text = "User log :  " + StaticClass.CurrentPlayerName;
            profilButton.interactable = (true);
            playGameButton.interactable = (true);
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



    

    //TODO: Add a parameter in order to know how to sort
    public void DisplayNews()
    {
        int index;
        int nbTotalNews = StaticClass.notificationList.Count;

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
            News n = StaticClass.notificationList[i];
            listBtnNews[i].gameObject.SetActive(true);
            listBtnNews[i].GetComponentInChildren<Text>().text = n.GetTitle() + "\n" + n.GetTagsToString() + " - " + n.GetDist() + "m .";
            
        }


    }






    public void OnClickNewsActivateBeacon(Button temp)
    {
        
        int index = listBtnNews.FindIndex(a => a == temp);
        News n = StaticClass.notificationList[index]; ///changer pour mettre la liste [10] de´s news "active" (en place actuellement)

        if( StaticClass.newsBeaconedList.Exists(x => x == n.GetId()) )
        {

            StaticClass.newsBeaconedList.Remove(n.GetId());
            temp.colors = ColorBlock.defaultColorBlock;
        }
        else
        {
            StaticClass.newsBeaconedList.Add(n.GetId());
            string color = StaticClass.tagPrefColorList[n.GetTags()[0]];  // we take the choosen color (by the player) of the "main" (first) tag of the news

            ColorBlock cb = temp.colors;
            cb.normalColor = color.ToColor();
            temp.colors = cb;
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
        // Disable VR to go to the dev mode since it's on a screen.
        DisableVR();
        SceneManager.LoadScene("DevMode", LoadSceneMode.Single);
    }

    public void Play()
    {
        SceneManager.LoadScene(4);
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




}

public class News
{
    private readonly uint id;   // uint for unsigned int;
    private readonly string title;
    private readonly uint distEuclFromSpawn;   // In AR, should be the player position, not the "spawn"
    private readonly List<string> tags;
    private readonly uint nbOfView;   // AKA Popularity
    private readonly DateTime date;

    public News(uint id, string title, uint distEuclFromSpawn, uint nbOfView, DateTime date, List<string> tags)
    {
        this.id = id;
        this.title = title;
        this.distEuclFromSpawn = distEuclFromSpawn;
        this.tags = tags;
        this.nbOfView = nbOfView;
        this.date = date;
    }


    // GETTERS
    public uint GetId() { return this.id; }
    public uint GetDist() { return this.distEuclFromSpawn; }
    public uint GetViews() { return this.nbOfView; }
    public string GetTitle() { return this.title; }
    public List<string> GetTags() { return this.tags; }
    public DateTime GetDate() { return this.date; }

    public string GetTagsToString()
    {
        string buff = "/ ";
        foreach(string s in this.tags)
        {
            buff += s+ " / ";
        }
        return buff;
    }

    //use for debug
    public override string ToString()  
    {
        return "N: " + this.id + "-" + this.title + " ("+this.distEuclFromSpawn+")";
    }
}

public static class ColorExtensions
{
    /// <summary>
    /// Convert string to Color (if defined as a static property of Color)
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color ToColor(this string color)
    {
        return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
    }
}