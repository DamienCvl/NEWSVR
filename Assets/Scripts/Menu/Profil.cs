using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;
using System.IO;
using Assets.Scripts.Core;
using UnityEditor;
using System;
using System.Linq;

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
    public ColorPicker picker;
    private List<GameObject> tagList = new List<GameObject>();
    private Color choice;



    // Start is called before the first frame update
    void Start()
    {
 
        AskStatData();
        AskCommentNumberData();
        AskCommentPositionData();
        Database.GetTagColors();
        DisplayTagsList();

        picker.onValueChanged.AddListener(color =>
        {
            choice = color;
        });
    }


    /*
     ColorBlock cb = copy.GetComponent<Button>().colors;
            cb.normalColor = color;
            cb.selectedColor = color;
            copy.GetComponent<Button>().colors = cb;
         */

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
        List<string> positions = Enum.GetNames(typeof(CommentGameObject.Positions)).ToList();
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


            Debug.Log(StaticClass.tagPrefColorList.Count);
            Debug.Log(s);

            Color c;
            if (StaticClass.tagPrefColorList.ContainsKey(s))
            {
                 c = StaticClass.tagPrefColorList[s];
            }
            else
            {
                c = Color.white;
            }

            //color the button with the pref color save
            ColorBlock cb = copy.GetComponent<Button>().colors;
            cb.normalColor = c;
            cb.selectedColor = c;
            copy.GetComponent<Button>().colors = cb;

            copy.SetActive(true);
            tagList.Add(copy);


            copy.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    if(picker.gameObject.active)
                    {
                        cb = copy.GetComponent<Button>().colors;
                        cb.normalColor = choice;
                        cb.selectedColor = choice;
                        copy.GetComponent<Button>().colors = cb;

                        picker.gameObject.SetActive(false);
                    }
                    else
                    {
                        picker.gameObject.SetActive(true);
                    }    
                }                
           );
        }
    }




    public void SaveButtonAction()
    {
        savePrompt.text = "";  //clear the prompt
        bool isColorSaved = true;
        StaticClass.tagPrefColorList.Clear();
        foreach (GameObject ob in tagList)
        {
            string text = ob.GetComponentInChildren<Text>().text;
            Color color = ob.GetComponent<Button>().colors.normalColor;
            StaticClass.tagPrefColorList.Add(text, color);
            if (!Database.SaveTagColorChoice(text, ColorUtility.ToHtmlStringRGB(color))) {
                isColorSaved = false;
                Debug.Log(text);
                break;  // if one time the color save process failed, stop the loop
            } 
        }

        if (isColorSaved && Database.PrefSucessfullySaved(Convert.ToInt32(cmtNumbersDD.options[cmtNumbersDD.value].text), cmtPositionDD.value))
        {
            

            savePrompt.color = Color.green;
            savePrompt.text = "Sucessfuly saved !";
        }
        else
        {
            savePrompt.color = Color.red;
            savePrompt.text = "Something wrong append ...";
            Debug.Log(isColorSaved);
        }
    }

    public void GoBackToMenu()
    {
        StaticClass.GoBackToMenu();
    }

}
