using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.UI;
using System;
using MySql.Data.MySqlClient;
using System.IO;


public class MainMenu : Connection
{
   
    public Button playGameButton;
    public Button profilButton;
    public List<News> notificationList = new List<News>();
    public const double SPAWN_X = -95.7;
    public const double SPAWN_Z = 87.3;

    public Text newsPrompt;

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
        ConnectDB();
        GenerateNewsList();
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

    // Euclidian distance
    public uint Distance(double x1, double y1, double x2, double y2) => Convert.ToUInt32(Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))));

    //Take qll the news from the db
    public void GenerateNewsList()
    {
        MySqlCommand cmdSQL = new MySqlCommand("SELECT NEWS.idNews, NEWS.title, NEWS.positionX, NEWS.positionZ, NEWS.nbView ,NEWS.creationDate FROM NEWS;", con);
        MySqlDataReader reader = cmdSQL.ExecuteReader();

        uint distEucli;
     
        List<string> tagsTemp = new List<string>();

        try
        {
            if (reader.HasRows)
            {
                newsPrompt.text = "News !!!";
                while (reader.Read())
                {
                    distEucli = Distance(reader.GetDouble(2), reader.GetDouble(3), SPAWN_X, SPAWN_Z);  // euclidian distance  from the spawn point                                  
                    notificationList.Add(new News(reader.GetUInt32(0), reader.GetString(1), distEucli, reader.GetUInt32(4), reader.GetDateTime(5), tagsTemp));
                }
                reader.Dispose();
            }
            else
            {
                newsPrompt.text = "No news ...";
            }
        }
        catch (IOException ex)
        {
            state.color = Color.red;
            state.text = ex.ToString();
        }
        reader.Dispose();
        cmdSQL.Dispose();


        uint idNewsTemp = 1;
        MySqlCommand cmdSQLtags = new MySqlCommand("SELECT * FROM TOPICS WHERE idNews = @dbIdNews ;", con);
        cmdSQLtags.Parameters.AddWithValue("@dbIdNews", idNewsTemp);
        MySqlDataReader readerTags = cmdSQLtags.ExecuteReader();

        try
        {
            foreach (News n in notificationList)
            {
                if (readerTags.HasRows)
                {
                    while (readerTags.Read())
                    {
                       n.GetTags().Add(readerTags.GetString(1));
                    }
                }

                idNewsTemp++;
            }
        }
        catch (IOException ex)
        {
            state.color = Color.red;
            state.text = ex.ToString();
        }
        
        readerTags.Dispose();
        cmdSQL.Dispose();
    }

    //TODO: Add a parameter in order to know how to sort
    public void DisplayNews()
    {
        int index;
        int nbTotalNews = notificationList.Count;

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
            News n = notificationList[i];
            listBtnNews[i].gameObject.SetActive(true);
            listBtnNews[i].GetComponentInChildren<Text>().text = n.GetTitle() + "\n" + n.GetTagsToString() + " - " + n.GetDist() + "m .";
            
        }


    }






    public void OnClickNewsActivateBeacon(Button temp)
    {
        
        int index = listBtnNews.FindIndex(a => a == temp);
        News n = notificationList[index]; ///changer pour mettre la liste [10] de´s news "active" (en place actuellement)

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