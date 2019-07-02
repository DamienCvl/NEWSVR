using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using System.IO;
using MySql.Data.MySqlClient;

// Handle the delete of comment in the database when you click the red cross and the verification panel.

public class DevModeVerifComments : Connection
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
        ConnectDB();
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
        
        MySqlCommand cmdVerifCom = new MySqlCommand("DELETE FROM COMMENTS WHERE ID = @dbIdComment;", con);
        cmdVerifCom.Parameters.AddWithValue("@dbIdComment", idComments);

        try
        {
            cmdVerifCom.ExecuteReader();
        }
        catch (IOException ex)
        {
            Debug.Log(ex.ToString());
        }

        cmdVerifCom.Dispose();
        con.Dispose();
    }
}
