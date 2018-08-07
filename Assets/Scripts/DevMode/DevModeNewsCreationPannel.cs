using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;

public class DevModeNewsCreationPannel : MonoBehaviour {

    public Button Cancel;
    public Button OK;
    public InputField Title;
    public InputField TextNews;
    public GameObject NewsPlacementManager;

	// Use this for initialization
	void Start () {
        
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
        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "INSERT INTO NEWS (TITLE,TEXT,POSITIONX,POSITIONZ) VALUES(\"" + title + "\", \"" + text + "\", \"" + posX + "\", \"" + posZ + "\"); ";
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
