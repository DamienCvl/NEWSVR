using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;

// Handle the delete of comment in the database when you click the red cross and the verification panel.

public class DevModeVerifComments : MonoBehaviour
{

    private int idComments;
    private GameObject viewToDestroy;

    public Button YesButton;
    public Button NoButton;
    public GameObject ViewVerif;
    public Text text;


    // Use this for initialization
    void Start()
    {
        YesButton.onClick.AddListener(YesAction);
        NoButton.onClick.AddListener(NoAction);
    }

    public void SetIdComments(int id, GameObject toDestroy)
    {
        idComments = id;
        viewToDestroy = toDestroy;
        text.text = "This action will destroy the comment.\n\nAre you sure ? ";
        ViewVerif.SetActive(true);
    }

    private void YesAction()
    {
        DeleteComments();
        Destroy(viewToDestroy);
        ViewVerif.SetActive(false);
    }

    private void NoAction()
    {
        ViewVerif.SetActive(false);
    }

    private void DeleteComments()
    {
        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "DELETE FROM COMMENTS WHERE ID = \"" + idComments + "\";";
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
