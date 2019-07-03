using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;
using Assets.Scripts.Core;

/*
 * Handle the creation of all the news when you open the scene TownSimulation.
 * There is a boolean used to see if a news is open in the scene, with this you can't open two news at the same time.
 * Since this script is always there, it deals with the escape action to return to the menu too.
 */

public class NewsPlacement : MonoBehaviour {

    private GameObject NewsPrefabs;
    private GameObject EveryNews;

    public bool aNewsIsOpen;

    // Use this for initialization
    void Start () {

        EveryNews = GameObject.Find("EveryNews");
        NewsPrefabs = (GameObject)Resources.Load("Prefabs/News/News", typeof(GameObject));
        // At first, no news is open and we have to pick up a sphere to go in one
        aNewsIsOpen = true;

        Database.ConnectDB();
        MySqlCommand cmdSQL = new MySqlCommand("SELECT NEWS.idNews, NEWS.title, NEWS.text, NEWS.positionX, NEWS.positionZ FROM NEWS;", Database.con);
        MySqlDataReader reader = cmdSQL.ExecuteReader();


        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string title = reader.GetString(1);
            string text = reader.GetString(2);
            float positionX = reader.GetFloat(3);
            float positionZ = reader.GetFloat(4);

            var news = Instantiate(NewsPrefabs, EveryNews.transform);
            var newsScript = news.GetComponent<NewsCreate>();
            newsScript.createNews(id, title, text, positionX, positionZ);
        }
        reader.Close();
        reader = null;
        cmdSQL.Dispose();
        cmdSQL = null;
        Database.con.Close();
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}
