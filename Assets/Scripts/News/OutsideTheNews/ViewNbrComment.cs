using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;

/*
 * This handles the view of the number of comment of the news item attached to it.
 * Used by NewsSphere.
 */

public class ViewNbrComment : MonoBehaviour {

    public GameObject Title;

    public void ReadCommentNbr()
    {

        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        // Change the view in the game
        string sqlQuery = "SELECT COMMENTNBR FROM NEWS WHERE TITLE = \"" + Title.GetComponent<TextMesh>().text.ToString() + "\";";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            var number = reader.GetValue(0);

            this.GetComponent<TextMesh>().text = number.ToString();

        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }
}
