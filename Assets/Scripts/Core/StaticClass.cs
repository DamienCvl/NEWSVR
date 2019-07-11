using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Can be called from everywhere easily. Used only to get the current player name in TownSimulation that have been choose in the menu.

public static class StaticClass {

    // Player infos
    public static string CurrentPlayerName = "";
    public static uint CurrentPlayerId;


    // Comments settings
    public static uint nbrCommentDisplayed;
    public static NewsComment.Positions CommentPosition
    {
        set
        {
            NewsComment.SetCommentsPosition(value);
        }
    }

    // News loaded
    public static List<News> newsList = new List<News>();

    public  const float SPAWN_X = -95.7f;
    public  const float SPAWN_Z = 87.3f;
    
    // Tags settings
    public static List<uint> newsBeaconedList = new List<uint>();
    public static Dictionary<string, Color> tagPrefColorList = new Dictionary<string, Color>();


    //return the uclidian distance between a new's location and the spawn
    public static uint DistanceFromSpawn(float x, float y) => Convert.ToUInt32(Math.Sqrt(((x - SPAWN_X) * (x - SPAWN_X) + (y - SPAWN_Z) * (y - SPAWN_Z))));

    public static void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
