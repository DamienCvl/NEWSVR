
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;
using System.IO;
using System;

public class Login : Connection
{
    public InputField logNameField;
    public InputField logPasswordField;

    public Button logInButton;

    public Text logStateTxt;


    // Start is called before the first frame update
    void Start()
    {
        ConnectDB();
        logInButton.onClick.AddListener(LogInButtonAction);
    }

    private void Update()
    {
        VerifyInputs();
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void VerifyInputs()
    {
        logInButton.interactable = (logNameField.text.Length >= 1 && logPasswordField.text.Length >= 8);
    }

    private void LogInButtonAction()
    {
        MySqlCommand cmdSQL = new MySqlCommand("SELECT idPlayer FROM PLAYERS WHERE name = @dbUserName AND password = @dbUserMDP;", con);
        cmdSQL.Parameters.AddWithValue("@dbUserName", logNameField.text);
        cmdSQL.Parameters.AddWithValue("@dbUserMDP", logPasswordField.text);
        MySqlDataReader reader = cmdSQL.ExecuteReader();

        try
        {
            if (reader.Read())
            {
                StaticClass.CurrentPlayerId = reader.GetUInt32(0);
                Debug.Log(StaticClass.CurrentPlayerId);
                StaticClass.CurrentPlayerName = logNameField.text;
                SceneManager.LoadScene(0);
                InitializeHomePageDataNeeded();
            }
            else
            {
                logStateTxt.text = "Wrong username or password.";
                
            }
            cmdSQL.Dispose();
        }
        catch (IOException ex)
        {
            state.color = Color.red;
            state.text = ex.ToString();
            cmdSQL.Dispose();
        }


        


    }

    private void InitializeHomePageDataNeeded()
    {
        GetTagColors();
    }

    private void GetTagColors()
    {
        MySqlCommand cmdSQL = new MySqlCommand("SELECT idPlayer FROM PLAYERS WHERE name = @dbUserName AND password = @dbUserMDP;", con);
        cmdSQL.Parameters.AddWithValue("@dbUserName", logNameField.text);
        MySqlDataReader reader = cmdSQL.ExecuteReader();

        try
        {
            if (reader.Read())
            {
                StaticClass.CurrentPlayerId = reader.GetUInt32(0);
                Debug.Log(StaticClass.CurrentPlayerId);
                StaticClass.CurrentPlayerName = logNameField.text;
                SceneManager.LoadScene(0);
                InitializeHomePageDataNeeded();
            }
            else
            {
                logStateTxt.text = "Wrong username or password.";

            }
            cmdSQL.Dispose();
        }
        catch (IOException ex)
        {
            state.color = Color.red;
            state.text = ex.ToString();
            cmdSQL.Dispose();
        }
    }
}
