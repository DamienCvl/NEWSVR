using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;

/*
 * This handles the number of view of the news item attavhed to it.
 * Used by NewsSphere.
 */

public class ViewNbrView : MonoBehaviour
{
    public GameObject Title;

    public void Add1ViewNbr()
    {
        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        // Add 1 to view nbr
        string sqlQuery = "UPDATE NEWS SET VIEWNBR = VIEWNBR + 1 WHERE TITLE = \"" + Title.GetComponent<TextMesh>().text.ToString() + "\";";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void ReadViewNbr()
    {

        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        // Change the view in the game
        string sqlQuery = "SELECT VIEWNBR FROM NEWS WHERE TITLE = \"" + Title.GetComponent<TextMesh>().text.ToString() + "\";";
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


