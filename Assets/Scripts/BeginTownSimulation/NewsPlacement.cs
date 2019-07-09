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

    public bool aNewsIsOpen;

    // Use this for initialization
    void Start () {
        
        // At first, no news is open and we have to pick up a sphere to go in one
        aNewsIsOpen = true;

        foreach (News news in StaticClass.newsList)
        {
            news.GenerateNewsGameObject(transform);
        }
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}
