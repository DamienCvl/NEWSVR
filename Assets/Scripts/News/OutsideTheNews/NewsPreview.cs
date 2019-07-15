using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsPreview : MonoBehaviour
{
    private NewsGameObject news = null;

    private Transform followHead;
    public Vector3 panelPreviewPostion = new Vector3(-0.3f, 0f, 0.7f);

    public TagListGameObject tagList;
    public ViewNbrComment viewNbrComment;
    public ViewNbrView viewNbrView;


    // Use to get infos from the news pointed on and display its
    public void SetPreviewInfos(NewsGameObject news)
    {
        this.news = news;

        tagList.newsGameObject = news;
        tagList.gameObject.SetActive(true);

        viewNbrView.GetComponent<TextMesh>().text = news.newsInfos.GetViews().ToString();
        viewNbrComment.GetComponent<TextMesh>().text = news.newsInfos.GetNbComment().ToString();
        
    }

    // Start is called before the first frame update
    void Awake()
    {
        followHead = GameObject.Find("FollowHead").transform;
        tagList = GetComponentInChildren<TagListGameObject>();
        viewNbrComment = GetComponentInChildren<ViewNbrComment>();
        viewNbrView = GetComponentInChildren<ViewNbrView>();
    }

    private void OnEnable()
    {
        if (news != null)
        {
            transform.Find("Panel/Title").GetComponent<Text>().text = news.TitleInNews.text;
            transform.Find("Panel/Infos").gameObject.GetComponent<Text>().text = news.content.text;
            transform.position = followHead.transform.TransformPoint(panelPreviewPostion);
            transform.rotation = Quaternion.LookRotation(transform.position - followHead.transform.position, Vector3.up);
        }
    }

    private void OnDisable()
    {
        foreach (Transform tag in tagList.gameObject.transform)
        {
            Destroy(tag.gameObject);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
