using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.SceneManagement;

public class NewsPlacement : MonoBehaviour {

    private GameObject NewsPrefabs;
    private GameObject EveryNews;

    public bool aNewsIsOpen;

    // Use this for initialization
    void Start () {

        EveryNews = GameObject.Find("EveryNews");
        NewsPrefabs = (GameObject)Resources.Load("Prefabs/News/News", typeof(GameObject));
        // At first, no news is open and we have to pick up a sphere to go in one
        aNewsIsOpen = true;

        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT TITLE, TEXT, POSITIONX, POSITIONZ FROM NEWS";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            string title = reader.GetString(0);
            string text = reader.GetString(1);
            float positionX = reader.GetFloat(2);
            float positionZ = reader.GetFloat(3);

            var news = Instantiate(NewsPrefabs, EveryNews.transform);
            var newsScript = news.GetComponent<NewsCreate>();
            newsScript.createNews(title, text, positionX, positionZ);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}
