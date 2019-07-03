using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Can be called from everywhere easily. Used only to get the current player name in TownSimulation that have been choose in the menu.

public static class StaticClass {

    public static string CurrentPlayerName = "";
    public static uint CurrentPlayerId;

    public static List<News> notificationList = new List<News>();
    public  const double SPAWN_X = -95.7;
    public  const double SPAWN_Z = 87.3;


    public static List<uint> newsBeaconedList;
    public static Dictionary<string, string> tagPrefColorList = new Dictionary<string, string>();


    //return the uclidian distance between a new's location and the spawn
    public static uint Distance(double x1, double y1, double x2, double y2) => Convert.ToUInt32(Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))));

    public static void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
