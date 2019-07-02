using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can be called from everywhere easily. Used only to get the current player name in TownSimulation that have been choose in the menu.

public static class StaticClass {

    public static string CurrentPlayerName = "";
    public static uint CurrentPlayerId;


    public static List<uint> newsBeaconedList;
    public static Dictionary<string, string> tagPrefColorList;

 
    
}
