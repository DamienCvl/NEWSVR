using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Valve.VR.InteractionSystem;

public class NewsCreate : MonoBehaviour {

    public TextMesh TitleOutNews;
    public Text TitleInNews;
    public Text text;

    private GameObject CommentPreFab;
    public GameObject ParentsOfComments;

    private void Awake()
    {
        CommentPreFab = (GameObject)Resources.Load("Prefabs/News/Comment", typeof(GameObject));
    }

    public void createNews(string newTitle, string newText, float newPositionX, float newPositionZ)
    {
        TitleOutNews.text = newTitle;
        TitleInNews.text = newTitle;
        text.text = newText;
        this.gameObject.transform.position = new Vector3(newPositionX, 0 , newPositionZ);
        createComments(newTitle);
    }

    private void createComments(string newTitle)
    {
        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT TEXT, AUTHOR, ID FROM COMMENTS WHERE NEWSATTACHED = \"" + newTitle + "\" ORDER BY CREATIONDATE DESC;";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            string text = reader.GetString(0);
            string author = reader.GetString(1);
            int id = reader.GetInt32(2);

            var comment = Instantiate(CommentPreFab, ParentsOfComments.transform);
            comment.GetComponent<NewsComment>().FillText(text, newTitle);
            comment.GetComponent<NewsComment>().FillAuthor(author);
            comment.GetComponent<NewsComment>().id = id;
            comment.GetComponent<NewsComment>().DestroyButtons();
            if (author == StaticClass.CurrentPlayerName)
            {
                comment.GetComponent<NewsComment>().DeleteButton.SetActive(true);
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
