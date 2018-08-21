using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;

// Handles the number of angry on the reaction box.

public class AngryNbrView : MonoBehaviour
{

    public Text Title;

    private void OnEnable()
    {
        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT ANGRYNBR FROM NEWS WHERE TITLE = \"" + Title.text.ToString() + "\";";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            var number = reader.GetValue(0);

            this.GetComponent<Text>().text = number.ToString();

        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
