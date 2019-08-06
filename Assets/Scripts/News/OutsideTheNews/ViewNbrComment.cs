using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Assets.Scripts.Core;

/*
 * This handles the view of the number of comment of the news item attached to it.
 * Used by NewsSphere.
 */

public class ViewNbrComment : MonoBehaviour {

    public NewsGameObject news;

    public void DisplayCommentNbr()
    {
        string nb = Database.ReadComntNum(news.Id);
        this.GetComponent<TextMesh>().text = nb;
        // Keep coherence with news object
        news.newsInfos.nbComment = Convert.ToUInt32(nb);
    }
}
