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

    private void Awake()
    {
        CommentPreFab = (GameObject)Resources.Load("Prefabs/News/Comment", typeof(GameObject));
    }

    public void CreateNews(News newsInfos)
    {
        Id = newsInfos.GetId();
        TitleOutNews.text = newsInfos.GetTitle();
        TitleInNews.text = TitleOutNews.text;
        content.text = newsInfos.GetContent();
        transform.position = newsInfos.GetPos();
        Tags = newsInfos.GetTags();
        CreateComments(Id);
        ActivateBeacon();
    }

    private void CreateComments(uint idNews)
    {
        // TODO : Requete pour aller chercher les commentaires sur la base puis les instancier
        /*
        var comment = Instantiate(CommentPreFab, ParentsOfComments.transform);
        comment.GetComponent<NewsComment>().FillText(idNews, text, newTitle);
        comment.GetComponent<NewsComment>().FillAuthor(author);
        comment.GetComponent<NewsComment>().id = id;
        comment.GetComponent<NewsComment>().DestroyButtons();

        // Add the delete comments option if the current player is the one who made the comments before.
        if (author == StaticClass.CurrentPlayerName)
        {
            comment.GetComponent<NewsComment>().DeleteButton.SetActive(true);
        }*/
    }


    private void ActivateBeacon()
    {
        if (StaticClass.newsBeaconedList.Contains(Id))
        {

        }
    }
}