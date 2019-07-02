using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Valve.VR.InteractionSystem;

/*
 * This handle the creation of one news item, it's called by NewsPlacement.
 * It fills the title inside and outside the news and the text of the article.
 * It creates all the comments of the news to by calling NewsComment.
 */

public class NewsCreate : MonoBehaviour {

    public TextMesh TitleOutNews;
    public Text TitleInNews;
    public Text text;
    public int idNews;

    private GameObject CommentPreFab;
    public GameObject ParentsOfComments;

    private void Awake()
    {
        CommentPreFab = (GameObject)Resources.Load("Prefabs/News/Comment", typeof(GameObject));
    }

    public void createNews(int idN, string newTitle, string newText, float newPositionX, float newPositionZ)
    {
        idNews = idN;
        TitleOutNews.text = newTitle;
        TitleInNews.text = newTitle;
        text.text = newText;
        this.gameObject.transform.position = new Vector3(newPositionX, 0 , newPositionZ);
        createComments(idN, newTitle);
    }

    private void createComments(int idNews, string newTitle)
    {



        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        // ORDER BY CREATIONDATE DESC so that the most recent comment will be a bit nearer from you.
        string sqlQuery = "SELECT TEXT, AUTHOR, ID FROM COMMENTS WHERE NEWSATTACHED = \"" + newTitle + "\" ORDER BY CREATIONDATE DESC;";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            string text = reader.GetString(0);
            string author = reader.GetString(1);
            int id = reader.GetInt32(2);

            var comment = Instantiate(CommentPreFab, ParentsOfComments.transform);
            comment.GetComponent<NewsComment>().FillText(idNews, text, newTitle);
            comment.GetComponent<NewsComment>().FillAuthor(author);
            comment.GetComponent<NewsComment>().id = id;
            comment.GetComponent<NewsComment>().DestroyButtons();

            // Add the delete comments option if the current player is the one who made the comments before.
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
