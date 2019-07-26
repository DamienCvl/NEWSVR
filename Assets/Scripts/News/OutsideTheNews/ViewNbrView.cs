﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Assets.Scripts.Core;

/*
 * This handles the number of view of the news item attached to it.
 * Used by NewsSphere.
 */

public class ViewNbrView : MonoBehaviour
{
    public NewsGameObject news;

    public void ReadViewNbr()
    {
        this.GetComponent<TextMesh>().text = Database.ReadViewNum(news.Id);
    }
}


