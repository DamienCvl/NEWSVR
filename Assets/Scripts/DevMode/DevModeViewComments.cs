using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;

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
        // To put the comments at the right places
        currentPosition = new Vector3(17.0f, -135.0f, 0.0f);

        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT ID, NEWSATTACHED, AUTHOR, TEXT FROM COMMENTS;";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            //Instantiate each commentsData
            int id = reader.GetInt32(0);
            string information = "Title of news : " + reader.GetString(1) + ", Author : " + reader.GetString(2) + ", Text : " + reader.GetString(3);

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
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }
}

