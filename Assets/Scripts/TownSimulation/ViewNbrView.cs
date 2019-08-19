using UnityEngine;
using System;
using Assets.Scripts.Core;
using Assets.Scripts.TownSimulation.NewsGO;

/*
 * This handles the number of view of the news item attached to it.
 * Used by NewsSphere.
 */

namespace Assets.Scripts.TownSimulation
{
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
}