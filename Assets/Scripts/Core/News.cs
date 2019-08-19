using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.TownSimulation.NewsGO;

/*
 * This handle the creation of one news item, it's called by NewsPlacement.
 * It fills the title inside and outside the news and the text of the article.
 * It creates all the comments of the news to by calling NewsComment.
 */

namespace Assets.Scripts.Core
{
    public class News
    {
        private readonly uint id;   // uint for unsigned int;
        private readonly string title;
        private readonly string content;
        private readonly float posX;
        private readonly float posZ;
        private readonly uint distEuclFromSpawn;   // In AR, should be the player position, not the "spawn"
        private readonly List<string> tags;
        public uint nbOfView;  // AKA Popularity
        public uint nbComment;
        private readonly DateTime date;
        private readonly List<Media> medium;

        private readonly GameObject NewsPreFab = (GameObject)Resources.Load("Prefabs/News/News", typeof(GameObject));
        public GameObject NewsGameObject { get; private set; }


        public News(uint id, string title, string content, float posX, float posZ, uint nbOfView, uint nbComment, DateTime date, List<string> tags, List<Media> m)
        {
            this.id = id;
            this.title = title;
            this.content = content;
            this.posX = posX;
            this.posZ = posZ;
            this.distEuclFromSpawn = StaticClass.DistanceFromSpawn(posX, posZ);
            this.tags = tags;
            this.nbOfView = nbOfView;
            this.nbComment = nbComment;
            this.date = date;
            this.medium = m;
        }


        // GETTERS
        public uint GetId() { return this.id; }
        public uint GetDist() { return this.distEuclFromSpawn; }
        public string GetTitle() { return this.title; }
        public string GetContent() { return this.content; }
        public Vector3 GetPos() { return new Vector3(posX, 0f, posZ); }
        public List<string> GetTags() { return this.tags; }
        public DateTime GetDate() { return this.date; }
        public List<Media> GetMedium() { return this.medium; }

        public string GetTagsToString()
        {
            string buff = "/ ";
            foreach (string s in this.tags)
            {
                buff += s + " / ";
            }
            return buff;
        }

        //use for debug
        public override string ToString()
        {
            return "N: " + this.id + "-" + this.title + " (" + this.distEuclFromSpawn + ")";
        }


        public void GenerateNewsGameObject(Transform parent)
        {
            GameObject news = UnityEngine.Object.Instantiate(NewsPreFab, parent);
            NewsGameObject newsScript = news.GetComponent<NewsGameObject>();
            newsScript.CreateNews(this);
            this.NewsGameObject = news;
        }

        public Color GetNewsColor()
        {
            if (tags.Count > 0 && StaticClass.tagPrefColorList.ContainsKey(tags[0]))
            {
                return StaticClass.tagPrefColorList[tags[0]];
            }
            else
            {
                return StaticClass.tagDefaultColor;
            }
        }
    }
}
