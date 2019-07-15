using Assets.Scripts.Core;
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

    public GameObject DeleteButton;


    private LinkedList<Comment> oldCommentsList = new LinkedList<Comment>();
    private LinkedListNode<Comment> currentOldCommentDisplayed;


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
        transform.position = CommentGameObject.firstCommentPosition;
        transform.rotation = CommentGameObject.firstCommentRotation;
        transform.Translate(Vector3.left * 0.5f + Vector3.forward * 0.2f);

        LoadOldComments();
    }

    private void OnDisable()
    {
        oldCommentsList.Clear();
    }

    private void LoadOldComments()
    {
        for (int i = StaticClass.nbrCommentDisplayed; i < Comment.commentsList.Count; i++)
        {
            oldCommentsList.AddLast(Comment.commentsList[i]);
        }
        LoadAComment(oldCommentsList.First);
    }

    private void LoadAComment(LinkedListNode<Comment> cmt)
    {
        if (cmt != null)
        {
            currentOldCommentDisplayed = cmt;
            FillText(cmt.Value.Content);
            FillAuthor(cmt.Value.Author);
        }
    }

    public void DeleteComment()
    {
        currentOldCommentDisplayed.Value.Delete();
        oldCommentsList.Remove(currentOldCommentDisplayed);
        LoadAComment(oldCommentsList.First);
    }

    // Use in MicroComments script to fill the comment
    public void FillText(string text)
    {
        Text textComment = content.GetComponent<Text>();
        textComment.text = text;
    }

    public void FillAuthor(string author)
    {
        Author.text = author;
    }

    public void PreviousComment()
    {
        LoadAComment(currentOldCommentDisplayed.Previous);
    }
    public void NextComment()
    {
        LoadAComment(currentOldCommentDisplayed.Next);
    }
}
