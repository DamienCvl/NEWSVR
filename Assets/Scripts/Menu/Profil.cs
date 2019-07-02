using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;
using System.IO;

public class Profil : Connection
{
    public Text nameField;
    public Text viewsField;
    public Text commentField;
    public Text savePrompt;

    public Dropdown cmtPositionDD;
    public Dropdown cmtNumbersDD;



    // Start is called before the first frame update
    void Start()
    {
        ConnectDB();
        AskStatData();
        AskCommentNumberData();
        AskCommentPositionData();
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
        int.TryParse(SqlCmd("cmtNbShown"), out int res);
        cmtNumbersDD.value = res;
    }

    void AskCommentPositionData()
    {
        PopulatePositionList();
        int.TryParse(SqlCmd("cmtPositionPref"), out int res);
        cmtPositionDD.value = res;
    }

    void AskStatData()
    {
        nameField.text += StaticClass.CurrentPlayerName;
        viewsField.text += SqlCmd("nbOfView");
        commentField.text += SqlCmd("nbOfComment");
    }








    string SqlCmd(string selectName)
    {
        MySqlCommand cmdSQL = new MySqlCommand("SELECT " + selectName + " FROM PLAYERS WHERE name = '" + StaticClass.CurrentPlayerName + "'", con);
        MySqlDataReader reader = cmdSQL.ExecuteReader();

        try
        {
            reader.Read();
            string var = "" + reader.GetValue(0);
            cmdSQL.Dispose();
            return var;
        }
        catch(IOException ex)
        {
            cmdSQL.Dispose();
            return "" + ex;
        }
 
    }


    public void SaveButtonAction()
    {
        MySqlCommand cmdSQL = new MySqlCommand("UPDATE PLAYERS SET cmtNbShown = '"+ cmtNumbersDD.value + "', cmtPositionPref = '"+ cmtPositionDD.value + "' WHERE name = '" + StaticClass.CurrentPlayerName + "'; ", con);
        

        try
        {
           if (cmdSQL.ExecuteNonQuery() > 0)
            {
               savePrompt.color = Color.green;
               savePrompt.text = "Sucessfuly saved !";
            }

            cmdSQL.Dispose();
           
        }
        catch (IOException ex)
        {
            savePrompt.color = Color.red;
            savePrompt.text = "Something wrong append ...";
            cmdSQL.Dispose();
            Debug.Log(ex);
        }
    }

}
