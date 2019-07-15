using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * This handle the creation of one news item, it's called by NewsPlacement.
 * It fills the title inside and outside the news and the text of the article.
 * It creates all the comments of the news to by calling NewsComment.
 */

public class News
{
    private readonly uint id;   // uint for unsigned int;
    private readonly string title;
    private readonly string content;
    private readonly float posX;
    private readonly float posZ;
    private readonly uint distEuclFromSpawn;   // In AR, should be the player position, not the "spawn"
    private readonly List<string> tags;
    private readonly uint nbOfView;   // AKA Popularity
    private readonly uint nbComment;
    private readonly DateTime date;

    private readonly GameObject NewsPreFab = (GameObject)Resources.Load("Prefabs/News/News", typeof(GameObject));
    public GameObject NewsGameObject { get; private set; }


    public News(uint id, string title, string content, float posX, float posZ, uint nbOfView, uint nbComment, DateTime date, List<string> tags)
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
    }


    // GETTERS
    public uint GetId() { return this.id; }
    public uint GetDist() { return this.distEuclFromSpawn; }
    public uint GetViews() { return this.nbOfView; }
    public uint GetNbComment() { return this.nbComment; }
    public string GetTitle() { return this.title; }
    public string GetContent() { return this.content; }
    public Vector3 GetPos() { return new Vector3(posX, 0f, posZ); }
    public List<string> GetTags() { return this.tags; }
    public DateTime GetDate() { return this.date; }

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
}
