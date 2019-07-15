using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class NewsGameObject : MonoBehaviour
{

    public TextMesh TitleOutNews;
    public Text TitleInNews;
    public Text content;
    public uint Id { get; private set; }
    public List<string> Tags { get; private set; }

    private GameObject CommentPreFab;
    public GameObject ParentsOfComments;
    public GameObject beacon;

    [HideInInspector]
    public News newsInfos;

    private void Awake()
    {
        Tags = new List<string>();
        CommentPreFab = (GameObject)Resources.Load("Prefabs/News/Comment", typeof(GameObject));
    }

    public void CreateNews(News newsInfos)
    {
        this.newsInfos = newsInfos;
        Id = newsInfos.GetId();
        TitleOutNews.text = newsInfos.GetTitle();
        TitleInNews.text = TitleOutNews.text;
        content.text = newsInfos.GetContent();
        transform.position = newsInfos.GetPos();
        Tags = newsInfos.GetTags();
        ActivateBeacon();
    }


    private void ActivateBeacon()
    {
        if (StaticClass.newsBeaconedList.Contains(Id))
        {
            Color color1;
            Color color2;
            if (Tags.Count > 0 && StaticClass.tagPrefColorList.ContainsKey(Tags[0]))
            {
                color1 = StaticClass.tagPrefColorList[Tags[0]];
            }
            else
            {
                color1 = Color.white;
            }
            color2 = color1;
            color1.a = 255 / 255;
            color2.a = 30 / 255;
            beacon.gameObject.GetComponent<Renderer>().material.SetColor("_Color1", color1);
            beacon.gameObject.GetComponent<Renderer>().material.SetColor("_Color2", color2);
            beacon.SetActive(true);
        }
    }
}