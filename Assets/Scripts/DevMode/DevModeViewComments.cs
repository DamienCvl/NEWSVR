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
     * Create all the rows of comments in the comments view when you enter the devmode scene.
     * I manage something for the placement in the scroll view but it's not perfect.
     */

    public class DevModeViewComments : MonoBehaviour
    {

        private GameObject CommentsDataPrefab;
        public RectTransform ParentsOfCommentsDatas;

        [HideInInspector]
        public Vector3 currentPosition;

        private void Awake()
        {
            CommentsDataPrefab = (GameObject)Resources.Load("Prefabs/DevMode/CommentsDatas", typeof(GameObject));
        }

        // Use this for initialization
        void Start()
        {
            Database.ConnectDB();
            // To put the comments at the right places
            currentPosition = new Vector3(17.0f, -135.0f, 0.0f);

            MySqlCommand dbcmd = new MySqlCommand("SELECT C.idComment, N.title, P.name, C.text, C.date FROM COMMENTS AS C JOIN PLAYERS AS P ON C.idPlayer = P.idPlayer JOIN NEWS AS N ON C.idNews = N.idNews ORDER BY C.idComment;", Database.con);
            MySqlDataReader reader = dbcmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    //Instantiate each commentsData
                    int id = reader.GetInt32(0);
                    string information = "Title of news : " + reader.GetString(1) + ", Author : " + reader.GetString(2) + ", Text : " + reader.GetString(3) + ", Created : " + reader.GetDateTime(4).ToString();

                    var commentsData = Instantiate(CommentsDataPrefab, ParentsOfCommentsDatas.transform);
                    commentsData.GetComponent<DevModeCommentsInstantiate>().FillText(id, information);
                    commentsData.GetComponent<DevModeCommentsInstantiate>().ChangePosition(currentPosition);

                    // I don't really understand how sizeDelta works but with this you can see everything so it's ok for the moment
                    currentPosition += new Vector3(0.0f, 30.0f, 0.0f);
                    if (currentPosition.y > 134.0f)
                    {
                        ParentsOfCommentsDatas.sizeDelta = new Vector2(ParentsOfCommentsDatas.sizeDelta.x, ParentsOfCommentsDatas.sizeDelta.y + 60f);
                        currentPosition += new Vector3(0.0f, -30f, 0.0f);
                    }
                }
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
            }

            dbcmd.Dispose();
            Database.con.Dispose();



        }
    }

}