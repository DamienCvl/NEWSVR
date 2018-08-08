using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class DevModeViewPlayers : MonoBehaviour
{

    private GameObject PlayersDataPrefab;
    public RectTransform ParentsOfPlayersDatas;

    [HideInInspector]
    public Vector3 currentPosition;

    private void Awake()
    {
        PlayersDataPrefab = (GameObject)Resources.Load("Prefabs/DevMode/PlayersDatas", typeof(GameObject));
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
        string sqlQuery = "SELECT NAME, NBRNEWSOPEN, NBRSAD, NBRHAPPY, NBRSURPRISE, NBRANGRY, NBRCOMMENT, CREATIONDATE FROM PLAYER;";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            //Instantiate each newsData
            string name = reader.GetString(0);
            string information = "Name : " + reader.GetString(0) + ", News opened : " + reader.GetValue(1).ToString() + ", Sad : " + reader.GetValue(2).ToString() + ", Happy : " + reader.GetValue(3).ToString() + ", Surprise : " + reader.GetValue(4).ToString() + ", Angry : " + reader.GetValue(5).ToString() + ", Comments : " + reader.GetValue(6).ToString() + ", Created : " + reader.GetString(7);

            var playersData = Instantiate(PlayersDataPrefab, ParentsOfPlayersDatas.transform);
            playersData.GetComponent<DevModePlayersInstantiate>().FillName(name, information);
            playersData.GetComponent<DevModePlayersInstantiate>().ChangePosition(currentPosition);

            // I don't really understand how sizeDelta works but with this you can see every news so it's ok for the moment
            currentPosition += new Vector3(0.0f, 30.0f, 0.0f);
            if (currentPosition.y > 134.0f)
            {
                ParentsOfPlayersDatas.sizeDelta = new Vector2(ParentsOfPlayersDatas.sizeDelta.x, ParentsOfPlayersDatas.sizeDelta.y + 60f);
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
