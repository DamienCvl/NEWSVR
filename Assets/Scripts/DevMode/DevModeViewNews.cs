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
     * Create all the rows of news in the news view when you enter the devmode scene.
     * I manage something for the placement in the scroll view but it's not perfect.
     */

    public class DevModeViewNews : MonoBehaviour
    {

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
            Database.ConnectDB();

            // To put the news at the right places
            currentPosition = new Vector3(17.0f, -135.0f, 0.0f);

            MySqlCommand dbcmd = new MySqlCommand("SELECT idNews, title, nbView, nbSad, nbHappy, nbAngry, nbSurprised, nbComment, creationDate FROM NEWS;", Database.con);
            MySqlDataReader reader = dbcmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    //Instantiate each newsData
                    int id = reader.GetInt32(0);
                    string title = reader.GetString(1);
                    string information = "Title : " + reader.GetString(1) + ", View number : " + reader.GetValue(2).ToString() + ", Sad number : " + reader.GetValue(3).ToString() + ", Happy number : " + reader.GetValue(4).ToString() + ", Angry number : " + reader.GetValue(5).ToString() + ", Surprise number : " + reader.GetValue(6).ToString() + ", Comments number : " + reader.GetValue(7).ToString() + ", Created : " + reader.GetDateTime(8).ToString(); ;

                    var newsData = Instantiate(NewsDataPrefab, ParentsOfNewsDatas.transform);
                    newsData.GetComponent<DevModeNewsInstantiate>().FillTitle(id, title, information);
                    newsData.GetComponent<DevModeNewsInstantiate>().ChangePosition(currentPosition);

                    // I don't really understand how sizeDelta works but with this you can see every news so it's ok for the moment
                    currentPosition += new Vector3(0.0f, 30.0f, 0.0f);
                    if (currentPosition.y > 134.0f)
                    {
                        ParentsOfNewsDatas.sizeDelta = new Vector2(ParentsOfNewsDatas.sizeDelta.x, ParentsOfNewsDatas.sizeDelta.y + 60f);
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
