using System.Collections;
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
        string nb = Database.ReadViewNum(news.Id);
        this.GetComponent<TextMesh>().text = nb;
        // Keep coherence with news object
        news.newsInfos.nbOfView = Convert.ToUInt32(nb);
    }
}


