using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;
using System.IO;
using Assets.Scripts.Core;
using UnityEditor;

public class Profil : MonoBehaviour
{
    public Text nameField;
    public Text viewsField;
    public Text commentField;
    public Text savePrompt;

    public Dropdown cmtPositionDD;
    public Dropdown cmtNumbersDD;

    //tags list
    public GameObject tagTemplate;
    public GameObject content;



    // Start is called before the first frame update
    void Start()
    {
 
        AskStatData();
        AskCommentNumberData();
        AskCommentPositionData();
        DisplayTagsList();
    }


    // Update is called once per frame
    void Update()
    {
     

        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene(0);
        }
    }



    /** DROPDOWN INITIALISATION **/
    void PopulatePositionList()
    {
        List<string> positions = new List<string>() { "Left","Rigth" };
        cmtPositionDD.AddOptions(positions);
    }

    void PopulateDisplayNumberList()
    {
        List<string> display = new List<string>() { "5", "6", "7", "8", "9" };
        cmtNumbersDD.AddOptions(display);
    }


    /***** ASK THE DB AND DISPLAY DATA *****/
    void AskCommentNumberData()
    {
        PopulateDisplayNumberList();
        int.TryParse(Database.SqlCmd("cmtNbShown"), out int res);
        cmtNumbersDD.value = res;
    }

    void AskCommentPositionData()
    {
        PopulatePositionList();
        int.TryParse(Database.SqlCmd("cmtPositionPref"), out int res);
        cmtPositionDD.value = res;
    }

    void AskStatData()
    {
        nameField.text += StaticClass.CurrentPlayerName;
        viewsField.text += Database.SqlCmd("nbOfView");
        commentField.text += Database.SqlCmd("nbOfComment");
    }


    public void DisplayTagsList()
    {
        

        foreach(string s in Database.GetTags())
        {
            var copy = Instantiate(tagTemplate);
            copy.transform.parent = content.transform;
            copy.transform.GetComponentInChildren<Text>().text = s;
            copy.SetActive(true);

            copy.GetComponent<Button>().onClick.AddListener(
                () =>
                {
              
                }                
           );
        }
    }




    public void SaveButtonAction()
    {
        if (Database.PrefSucessfullySaved(cmtNumbersDD.value, cmtPositionDD.value))
        {
            savePrompt.color = Color.green;
            savePrompt.text = "Sucessfuly saved !";
        }
        else
        {
            savePrompt.color = Color.red;
            savePrompt.text = "Something wrong append ...";
        }
    }

    public void GoBackToMenu()
    {
        StaticClass.GoBackToMenu();
    }

}
