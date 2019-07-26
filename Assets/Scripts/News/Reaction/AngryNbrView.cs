using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Assets.Scripts.Core;

// Handles the number of angry on the reaction box.

public class AngryNbrView : MonoBehaviour
{

    private void OnEnable()
    {
        this.GetComponent<Text>().text = Database.NumOfReatcionToNews("Angry", StaticClass.CurrentNewsId); 
    }
}
