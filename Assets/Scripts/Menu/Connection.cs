using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.IO;
using UnityEngine.SceneManagement;


public class Connection : MonoBehaviour
{

    public Text state;

    public MySqlConnection con;

    public void ConnectDB()
    {
        string constr = "Server='mysql-levelup.alwaysdata.net';DATABASE='levelup_newsvr';User ID='levelup';Password='LevelUp20!)';Pooling=true;Charset=utf8;";
        try
        {
            con = new  MySqlConnection(constr);
            con.Open();
            

        }
        catch(IOException ex)
        {
            state.text = ex.ToString();
        }
    }

    void Update()
    {
        state.text = con.State.ToString();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Shutdown Connexion");

        if (con != null && con.State.ToString() != "Closed")
        {
            con.Close();
        }
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
