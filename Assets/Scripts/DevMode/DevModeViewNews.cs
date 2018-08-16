using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;

/*
 * Create all the rows of news in the news view when you enter the devmode scene.
 * I manage something for the placement in the scroll view but it's not perfect.
 */

public class DevModeViewNews : MonoBehaviour {

    private GameObject NewsDataPrefab;
    public RectTransform ParentsOfNewsDatas;

     [HideInInspector]
    public Vector3 currentPosition;

    private void Awake()
    {
        NewsDataPrefab = (GameObject)Resources.Load("Prefabs/DevMode/NewsDatas", typeof(GameObject));
    }

    // Use this for initialization
    void Start()
    {
        // To put the news at the right places
        currentPosition = new Vector3(17.0f, -135.0f, 0.0f);

        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT TITLE, VIEWNBR, SADNBR, HAPPYNBR, ANGRYNBR, SURPRISENBR, COMMENTNBR, CREATIONDATE FROM NEWS;";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            //Instantiate each newsData
            string title = reader.GetString(0);
            string information = "Title : " + reader.GetString(0) + ", View number : " + reader.GetValue(1).ToString() + ", Sad number : " + reader.GetValue(2).ToString() + ", Happy number : " + reader.GetValue(3).ToString() + ", Angry number : " + reader.GetValue(4).ToString() + ", Surprise number : " + reader.GetValue(5).ToString() + ", Comments number : " + reader.GetValue(6).ToString() + ", Created : " + reader.GetString(7);

            var newsData = Instantiate(NewsDataPrefab, ParentsOfNewsDatas.transform);
            newsData.GetComponent<DevModeNewsInstantiate>().FillTitle(title, information);           
            newsData.GetComponent<DevModeNewsInstantiate>().ChangePosition(currentPosition);

            // I don't really understand how sizeDelta works but with this you can see every news so it's ok for the moment
            currentPosition += new Vector3(0.0f, 30.0f, 0.0f);
            if (currentPosition.y > 134.0f)
            {
                ParentsOfNewsDatas.sizeDelta = new Vector2(ParentsOfNewsDatas.sizeDelta.x, ParentsOfNewsDatas.sizeDelta.y + 60f);
                currentPosition += new Vector3(0.0f, -30f, 0.0f);
            }
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }
}