using UnityEngine;
using System;
using Assets.Scripts.Core;
using Assets.Scripts.TownSimulation.NewsGO;

/*
 * This handles the view of the number of comment of the news item attached to it.
 * Used by NewsSphere.
 */

namespace Assets.Scripts.TownSimulation
{
    public class ViewNbrComment : MonoBehaviour
    {

        public NewsGameObject news;

        public void DisplayCommentNbr()
        {
            string nb = Database.ReadComntNum(news.Id);
            this.GetComponent<TextMesh>().text = nb;
            // Keep coherence with news object
            news.newsInfos.nbComment = Convert.ToUInt32(nb);
        }
    }
}