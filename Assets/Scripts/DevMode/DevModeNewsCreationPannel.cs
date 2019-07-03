using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.IO;
using Assets.Scripts.Core;

// Handle the creation of a news after you choose the position by clicking on the town in devmode.

public class DevModeNewsCreationPannel : MonoBehaviour
{

    public Button Cancel;
    public Button OK;
    public InputField Title;
    public InputField TextNews;
    public GameObject NewsPlacementManager;

	// Use this for initialization
	void Start () {
        Database.ConnectDB();
        Cancel.onClick.AddListener(CancelAction);
        OK.onClick.AddListener(OkAction);
    }

    private void OnDisable()
    {
        Title.text = "";
        TextNews.text = "";
    }

    void CancelAction()
    {
        NewsPlacementManager.GetComponent<DevModeCreateNews>().newsBeingCreated = false;
        this.gameObject.SetActive(false);
    }

    void OkAction()
    {
        // Create the news
        Debug.Log(NewsPlacementManager.GetComponent<DevModeCreateNews>().newsPos.x);
        CreateANews(Title.text.ToString(), TextNews.text.ToString(), NewsPlacementManager.GetComponent<DevModeCreateNews>().newsPos.x, NewsPlacementManager.GetComponent<DevModeCreateNews>().newsPos.z);

        // Say that we are not creating a news at the moment so that we can click on the map
        NewsPlacementManager.GetComponent<DevModeCreateNews>().newsBeingCreated = false;
        this.gameObject.SetActive(false);
    }

    void CreateANews(string title, string text, float posX, float posZ)
    {

        MySqlCommand cmdCreateNews = new MySqlCommand("INSERT INTO NEWS(title, text, author, creationDate, nbView, nbComment, nbHappy, nbSad, nbAngry, nbSurprised, positionX, positionZ, laserTarget) VALUES(@dbNewsCreaTitle,@dbNewsCreaText,@dbNewsCreaAuthor, @dbNewsCreaDate,0,0,0,0,0,0,@dbNewsCreaPosiX, @dbNewsCreaPosiZ,'');", Database.con);
        cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaTitle", title);
        cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaText", text);
        cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaAuthor", StaticClass.CurrentPlayerName);
        cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaDate", DateTime.Now);
        cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaPosiX", posX);
        cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaPosiZ", posZ);

        try
        {
            cmdCreateNews.ExecuteReader();
        }
        catch (IOException ex)
        {
            Debug.Log(ex.ToString());
        }

        cmdCreateNews.Dispose();
        cmdCreateNews = null;
        Database.con.Close();
      
       
    }
}
