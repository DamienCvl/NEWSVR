using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using MySql.Data.MySqlClient;
using System.IO;
using Assets.Scripts.Core;

namespace Assets.Scripts.DevMode
{
    /*
     * Create all the rows of players in the players view when you enter the devmode scene.
     * I manage something for the placement in the scroll view but it's not perfect.
     */

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
            Database.ConnectDB();

            // To put the players at the right places
            currentPosition = new Vector3(17.0f, -135.0f, 0.0f);

            MySqlCommand dbcmd = new MySqlCommand("SELECT name, nbOfView, nbOfComment FROM PLAYERS;", Database.con);
            MySqlDataReader reader = dbcmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    //Instantiate each playersData
                    string name = reader.GetString(0);
                    string information = "Name : " + name + ", News opened : " + reader.GetValue(1).ToString() + ", Comments : " + reader.GetValue(2).ToString();

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
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
            }

            dbcmd.Dispose();
            dbcmd = null;
            reader.Close();
            reader = null;
            Database.con.Dispose();




        }
    }
}