using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
namespace Assets.Scripts.DevMode
{
    // Handle the delete of player in the database when you click the red cross and the verification panel.
    // When you delete a player, it deletes all the comments linked to him too.

    public class DevModeVerifPlayers : MonoBehaviour
    {

        private string namePlayers;
        private GameObject viewToDestroy;

        public Button YesButton;
        public Button NoButton;
        public GameObject ViewVerif;
        public Text text;


        // Use this for initialization
        void Start()
        {
            YesButton.onClick.AddListener(YesAction);
            NoButton.onClick.AddListener(NoAction);
        }

        public void SetNamePlayer(string name, GameObject toDestroy)
        {
            namePlayers = name;
            viewToDestroy = toDestroy;
            text.text = "This action will destroy the player " + name + " and all the comments that he made.\n\nAre you sure ? ";
            ViewVerif.SetActive(true);
        }

        private void YesAction()
        {
            DeleteComments();
            DeletePlayers();
            Destroy(viewToDestroy);
            ViewVerif.SetActive(false);
        }

        private void NoAction()
        {
            ViewVerif.SetActive(false);
        }

        private void DeletePlayers()
        {
            string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "DELETE FROM PLAYER WHERE NAME = \"" + namePlayers + "\";";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }

        private void DeleteComments()
        {
            string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "DELETE FROM COMMENTS WHERE AUTHOR = \"" + namePlayers + "\";";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
    }

}