﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.IO;
using Assets.Scripts.Core;

// Handle the creation of a news after you choose the position by clicking on the town in devmode.

public class DevModeNewsCreationPannel : MonoBehaviour
{

    public Button Cancel;
    public Button OK;
    public InputField Title;
    public InputField TextNews;
    public GameObject NewsPlacementManager;

	// Use this for initialization
	void Start () {        
        Cancel.onClick.AddListener(CancelAction);
        OK.onClick.AddListener(OkAction);
    }

    private void OnDisable()
    {
        Title.text = "";
        TextNews.text = "";
    }

    void CancelAction()
    {
        NewsPlacementManager.GetComponent<DevModeCreateNews>().newsBeingCreated = false;
        this.gameObject.SetActive(false);
    }

    void OkAction()
    {
        // Create the news
        Debug.Log(NewsPlacementManager.GetComponent<DevModeCreateNews>().newsPos.x);
        Database.CreateANews(Title.text.ToString(), TextNews.text.ToString(), NewsPlacementManager.GetComponent<DevModeCreateNews>().newsPos.x, NewsPlacementManager.GetComponent<DevModeCreateNews>().newsPos.z);

        // Say that we are not creating a news at the moment so that we can click on the map
        NewsPlacementManager.GetComponent<DevModeCreateNews>().newsBeingCreated = false;
        this.gameObject.SetActive(false);
    }

    
}
