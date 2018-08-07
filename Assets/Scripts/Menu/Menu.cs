using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class Menu : MonoBehaviour {

    public Button DevMode;
    public Button Play;
    public InputField PlayerName;

    public GameObject Verif;
    public Button VerifYes;
    public Button VerifNo;
    private bool nameAlreadyTaken;

    // Use this for initialization
    void Start () {
        nameAlreadyTaken = false;
        DevMode.onClick.AddListener(DevModeAction);
        Play.onClick.AddListener(PlayAction);
        VerifYes.onClick.AddListener(YesAction);
        VerifNo.onClick.AddListener(NoAction);
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

    void DevModeAction()
    {
        DisableVR();
        SceneManager.LoadScene("DevMode", LoadSceneMode.Single);
    }

    void PlayAction()
    { 
        VerifName(PlayerName.text);
        if(!nameAlreadyTaken)
        {
            string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "INSERT INTO PLAYER (NAME) VALUES (\"" + PlayerName.text + "\");";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            // Change the CurrentPlayerName in the StaticClass
            StaticClass.CurrentPlayerName = PlayerName.text;
            EnableVR();
            SceneManager.LoadScene("TownSimulation", LoadSceneMode.Single);
        }
    }

    void YesAction()
    {
        // Change the CurrentPlayerName in the StaticClass
        StaticClass.CurrentPlayerName = PlayerName.text;
        EnableVR();
        SceneManager.LoadScene("TownSimulation", LoadSceneMode.Single);
    }

    void NoAction()
    {
        Verif.SetActive(false);
        nameAlreadyTaken = false;
    }

    void VerifName(string name)
    {
        // If the name is already in the database, confirm with a message like " Name already taken, is this you? Yes, continue or No, cancel"
        // If No go back delte the message and nothing, if yes load the scene with this player datas
        string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT NAME FROM PLAYER WHERE NAME = \"" + name + "\";";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            if(reader.GetString(0) != null)
            {
                nameAlreadyTaken = true;
                Verif.SetActive(true);
            }
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    IEnumerator LoadDevice(string newDevice, bool enable)
    {
        XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        XRSettings.enabled = enable;
    }

    void EnableVR()
    {
        StartCoroutine(LoadDevice("OpenVR", true));
    }

    void DisableVR()
    {
        StartCoroutine(LoadDevice("", false));
    }
}
