
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;
using System.IO;
using System;
using Assets.Scripts.Core;

public class Login : MonoBehaviour
{
    public InputField logNameField;
    public InputField logPasswordField;

    public Button logInButton;

    public Text logStateTxt;


    // Start is called before the first frame update
    void Start()
    {
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
        if (Database.IsThisUserAnAuthenticPlayer(logNameField.text, logPasswordField.text))
        {
            StaticClass.CurrentPlayerName = logNameField.text;
            InitializeHomePageDataNeeded();
            SceneManager.LoadScene(0);
        }
        else
        {
            logStateTxt.text = "Wrong username or password.";
        }
    }

    private void InitializeHomePageDataNeeded()
    {
        Database.GetTagColors();
    }

    public void GoBackToMenu()
    {
        StaticClass.GoBackToMenu();
    }
}
