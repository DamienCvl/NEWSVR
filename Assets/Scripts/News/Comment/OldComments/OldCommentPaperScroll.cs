using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class OldCommentPaperScroll : MonoBehaviour
{

    public GameObject openScroll;
    public GameObject scrollPaper;

    private RectTransform rectScrollPaper;
    private float scaleX;

    public GameObject content;
    public TextMesh Author;

    [HideInInspector]
    public int id;
    public GameObject DeleteButton;

    [HideInInspector]
    public string titleOfNews;

    [HideInInspector]
    public string textOfComment;


    // Start is called before the first frame update
    void Start()
    {
        if (openScroll == null)
        {
            openScroll = transform.Find("OpenScroll").gameObject;
        }

        if (scrollPaper == null)
        {
            scrollPaper = transform.Find("ScrollPaper").gameObject;
        }

        rectScrollPaper = scrollPaper.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        scaleX = openScroll.GetComponent<OldCommentLinearDrive>().ScrollPosition * 1.0f;
        rectScrollPaper.localScale = new Vector3(scaleX, rectScrollPaper.localScale.y, rectScrollPaper.localScale.z);

    }

    private void OnEnable()
    {
        // Set position and rotation of the old comment scroll paper
        transform.position = NewsComment.nextCommentPosition;
        transform.rotation = NewsComment.nextCommentRotation;
        transform.Translate(Vector3.left + Vector3.forward * 0.2f);
    }

    private void OnDisable()
    {

    }

    // Use in MicroComments script to fill the comment
    public void FillText(string text, string title)
    {
        Text textComment = content.GetComponent<Text>();
        textComment.text = text;
        titleOfNews = title;
        textOfComment = text;
        Author.text = StaticClass.CurrentPlayerName;
    }

    public void FillAuthor(string author)
    {
        Author.text = author;
    }

    public void PreviousComment()
    {
        print("Previous");
        //TODO
        //Access the previous comment and load it
    }
    public void NextComment()
    {
        print("Next");
        //TODO
        //Access the next comment and load it
    }
}
