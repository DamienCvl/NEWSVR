using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Assets.Scripts.Core;

// Handles the number of happy on the reaction box.

public class HappyNbrView : MonoBehaviour
{

    public Text Title;
    public uint idN;

    private void OnEnable()
    {
        this.GetComponent<Text>().text = Database.NumOfReatcionToNews("Happy", idN);
    }
}

