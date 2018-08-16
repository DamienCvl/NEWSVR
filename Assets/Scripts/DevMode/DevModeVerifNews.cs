using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;

// Handle the delete of news in the database when you click the red cross and the verification panel.
// When you delete a news, it deletes all the comments linked to him too.

public class DevModeVerifNews : MonoBehaviour {

    private string nameNews;
    private GameObject viewToDestroy;

    public Button YesButton;
    public Button NoButton;
    public GameObject ViewVerif;
    public Text text;


    // Use this for initialization
    void Start () {
        YesButton.onClick.AddListener(YesAction);
        NoButton.onClick.AddListener(NoAction);
    }

    public void SetNameNews(string name, GameObject toDestroy)
    {
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
        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "DELETE FROM NEWS WHERE TITLE = \"" + nameNews + "\";";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    private void DeleteComments()
    {
        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "DELETE FROM COMMENTS WHERE NEWSATTACHED = \"" + nameNews + "\";";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
