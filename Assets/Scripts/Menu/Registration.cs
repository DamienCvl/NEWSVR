using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.IO;
using MySql.Data.MySqlClient;
using System;
using Assets.Scripts.Core;

public class Registration : MonoBehaviour
{
    

    public InputField nameField;
    public InputField passwordField;
    public InputField confirmPassField;

    public Button submitButton;

    public Text emptyRuleField;
    public Text lengthRuleField;
    public Text state;


    // Use this for initialization
    void Start()
    {
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


    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 1 && passwordField.text.Length >= 8 && confirmPassField.text.Length >= 8);
    }



    private void SubmitButtonAction()
    {
        if (passwordField.text == confirmPassField.text)
        {
            if (Database.VerifNameAvailable(nameField.text))
            {
                if(Database.InsertNewPlayer(nameField.text, passwordField.text))
                {
                    //initialize every colortags to white for the player
                    foreach (string s in Database.GetTags())
                    {
                        Database.InsertTagColorChoice(s, nameField.text);
                    }

                    state.color = Color.green;
                    state.text = "User created sucessfully.";
                    emptyRuleField.text = "";
                    lengthRuleField.text = "Press \"Esc\" and log in.";
                }
                else
                {
                    state.color = Color.red;
                    state.text = "Something Wrong append ...";
                }
            }
            else
            {
                state.text = "This name is already taken.";
            }
        }
        else
        {
            state.text = "Passwords are not matching.";
        }
        
    }


    public void GoBackToMenu()
    {
        StaticClass.GoBackToMenu();
    }
}
