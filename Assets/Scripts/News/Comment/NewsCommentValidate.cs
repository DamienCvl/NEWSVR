using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Assets.Scripts.Core;

/*
 * This handles the validate button that appeares when you speak in the microphone.
 * The comment will only be add to the database if the player click on this button. 
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class NewsCommentValidate : Grabbable
    {

        public GameObject Buttons;
        public GameObject Comment;

        // Action when you click validate, delete the two buttons
        private void ValidateAction()
        {
            // Retrieve the text of the comment and the title of the news
            var id = Comment.GetComponent<NewsComment>().id;
            var text = Comment.GetComponent<NewsComment>().textOfComment;

            Database.AddComment(id, text);
            Database.Add1CommentToPlayer();

            FillId();

            Destroy(Buttons);
            // Wait 1 second to delete the validate button to not cause bug
            Destroy(gameObject, 1);
        }

        

        

        private void FillId()
        {
            string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            // The Id increases automatically so if you selecte the max, you have the last one added.
            string sqlQuery = "SELECT ID FROM COMMENTS WHERE ID = (SELECT MAX(ID) FROM COMMENTS); ; ";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                int newId = reader.GetInt32(0);

                Comment.GetComponent<NewsComment>().id = newId;
                // If you just created it, it's yours so you can destroy it
                Comment.GetComponent<NewsComment>().DeleteButton.SetActive(true);
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }

        //-------------------------------------------------
        protected new void OnAttachedToHand(Hand hand)
        {
            base.OnAttachedToHand(hand);
            ValidateAction();
        }
    }
}
