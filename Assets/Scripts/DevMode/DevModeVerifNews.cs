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

// Handle the delete of news in the database when you click the red cross and the verification panel.
// When you delete a news, it deletes all the comments linked to him too.

public class DevModeVerifNews : MonoBehaviour
{

    private string nameNews;
    private int idNews;
    private GameObject viewToDestroy;

    public Button YesButton;
    public Button NoButton;
    public GameObject ViewVerif;
    public Text text;


    // Use this for initialization
    void Start () {
        Database.ConnectDB();
        YesButton.onClick.AddListener(YesAction);
        NoButton.onClick.AddListener(NoAction);
    }

    public void SetNameNews(int id,string name, GameObject toDestroy)
    {
        idNews = id;
        nameNews = name;
        viewToDestroy = toDestroy;
        text.text = "This action will destroy the news " + name + " and all the comments attached.\n\nAre you sure ? ";
        ViewVerif.SetActive(true);
    }

    private void YesAction()
    {
        DeleteComments();
        DeleteNews();
        Destroy(viewToDestroy);
        ViewVerif.SetActive(false);
    }

    private void NoAction()
    {
        ViewVerif.SetActive(false);
    }

    private void DeleteNews()
    {
        MySqlCommand cmdDeleteNews = new MySqlCommand("DELETE FROM NEWS WHERE idNews = @dbIdNews;", Database.con);
        cmdDeleteNews.Parameters.AddWithValue("@dbIdNews", idNews);

        try
        {
            cmdDeleteNews.ExecuteReader();
        }
        catch (IOException ex)
        {
            Debug.Log(ex.ToString());
        }

        cmdDeleteNews.Dispose();
        Database.con.Dispose();
    }

    private void DeleteComments()
    {
        MySqlCommand cmdDeleteComt = new MySqlCommand("DELETE FROM COMMENTS WHERE idNews = @dbIdNews;", Database.con);
        cmdDeleteComt.Parameters.AddWithValue("@dbIdNews", idNews);

        try
        {
            cmdDeleteComt.ExecuteReader();
        }
        catch (IOException ex)
        {
            Debug.Log(ex.ToString());
        }

        cmdDeleteComt.Dispose();
        Database.con.Dispose();
    }
}
