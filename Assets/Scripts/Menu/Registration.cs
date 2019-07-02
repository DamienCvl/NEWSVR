using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.IO;
using MySql.Data.MySqlClient;
using System;

public class Registration : Connection
{
    

    public InputField nameField;
    public InputField passwordField;
    public InputField confirmPassField;

    public Button submitButton;

    public Text emptyRuleField;
    public Text lengthRuleField;


    // Use this for initialization
    void Start()
    {
        ConnectDB();
        submitButton.onClick.AddListener(SubmitButtonAction);
    }

    private void Update()
    {
        VerifyInputs();
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene(0);
        }

    }

    /*
    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("mysql-levelup.alwaysdata.net");
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            Debug.Log("User created sucessfully.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
    */


    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 1 && passwordField.text.Length >= 8 && confirmPassField.text.Length >= 8);
    }

    bool VerifNameAvailable(string name)
    {
        string sqlCmdName = "SELECT name FROM PLAYERS WHERE name = @dbUserName;";
        MySqlCommand cmdVerifName = new MySqlCommand(sqlCmdName, con);
        cmdVerifName.Parameters.AddWithValue("@dbUserName", name);

        try
        {
            MySqlDataReader reader = cmdVerifName.ExecuteReader();
            if (reader.Read())
            {
                state.text = "This name is already taken.";
                reader.Close();
                reader = null;
                cmdVerifName.Dispose();
                return false;
            }
            else
            {
                cmdVerifName.Dispose();
                return true;
            }
            
        }
        catch (IOException ex)
        {
            state.text = ex.ToString();
            return false;
        }
        
    }

    private void SubmitButtonAction()
    {
        if (passwordField.text == confirmPassField.text)
        {
            if (VerifNameAvailable(nameField.text))
            {
                //string sqlCmdReg = "INSERT INTO PLAYERS VALUES (default,'" + nameField.text + "','" + passwordField.text + "',0,0,0,0,default)";   <- BEFORE
                //Now it's safe from SQL injections
                string sqlCmdReg = "INSERT INTO PLAYERS VALUES (default,@dbUserName,@dbUserMDP,0,0,0,0,default);";
                MySqlCommand cmdReg = new MySqlCommand(sqlCmdReg, con);
                cmdReg.Parameters.AddWithValue("@dbUserName", nameField.text);
                cmdReg.Parameters.AddWithValue("@dbUserMDP", passwordField.text);

                try
                {
                    cmdReg.ExecuteReader();
                    state.color = Color.green;
                    state.text = "User created sucessfully.";
                    emptyRuleField.text = "";
                    lengthRuleField.text = "Press \"Esc\" and log in.";
                }
                catch (IOException ex)
                {
                    state.color = Color.red;
                    state.text = ex.ToString();
                }

                cmdReg.Dispose();
            }
        }
        else
        {
            state.text = "Passwords are not matching.";
        }
        
    }
}
