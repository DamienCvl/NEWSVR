using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsPreview : MonoBehaviour
{
    private NewsGameObject news;

    private Transform followHead;
    public Vector3 panelPreviewPostion = new Vector3(-0.3f, 0f, 0.5f);

    public void SetNews(NewsGameObject news)
    {
        this.news = news;
        GetComponentInChildren<TagListGameObject>().newsGameObject = news;
    }

    // Start is called before the first frame update
    void Awake()
    {
        followHead = GameObject.Find("FollowHead").transform;
    }

    private void OnEnable()
    {
        transform.Find("Panel/Title").GetComponent<Text>().text = news.TitleInNews.text;
        transform.Find("Panel/Infos").gameObject.GetComponent<Text>().text = news.content.text;
        transform.position = followHead.transform.TransformPoint(panelPreviewPostion);
        transform.rotation = Quaternion.LookRotation(transform.position - followHead.transform.position, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
