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

    public Text actualCommentNumber;
    public Text totalCommentNumber;
    private int actualCommentNumberInt;
    private int totalCommentNumberInt;

    public GameObject arrowUp;
    public GameObject arrowDown;


    private LinkedList<Comment> oldCommentsList = new LinkedList<Comment>();
    private LinkedListNode<Comment> currentOldCommentDisplayed;


    // Start is called before the first frame update
    void Awake()
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
        scrollPaper.SetActive(false); // To avoid UI overlap
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
        transform.position = CommentGameObject.nextCommentPosition;
        transform.rotation = CommentGameObject.nextCommentRotation;
        transform.Translate(Vector3.left * 0.5f + Vector3.forward * 0.1f);

        LoadOldComments();

        actualCommentNumberInt = 1;
        actualCommentNumber.text = "1";
        totalCommentNumberInt = oldCommentsList.Count;
        totalCommentNumber.text = totalCommentNumberInt.ToString();
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

            if (Author.text == StaticClass.CurrentPlayerName)
            {
                DeleteButton.SetActive(true);
            }
            else
            {
                DeleteButton.SetActive(false);
            }
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

    // Call when ArrowUp is pressed
    public void PreviousComment()
    {
        LoadAComment(currentOldCommentDisplayed.Previous);

        // Activate ArrowDown when we are not on last comment anymore
        if (actualCommentNumberInt == totalCommentNumberInt)
            SetArrowActive(arrowDown, true);

        // Change number of current comment
        if (actualCommentNumberInt > 1)
        {
            actualCommentNumberInt--;
            actualCommentNumber.text = actualCommentNumberInt.ToString();
        }

        // Desactivate ArrowUp if we are on first comment
        if (actualCommentNumberInt == 1)
            SetArrowActive(arrowUp, false);
    }

    // Call when ArrowDown is pressed
    public void NextComment()
    {
        LoadAComment(currentOldCommentDisplayed.Next);

        // Activate ArrowUp when we are not on first comment anymore
        if (actualCommentNumberInt == 1)
            SetArrowActive(arrowUp, true);

        // Change number of current comment
        if (actualCommentNumberInt < totalCommentNumberInt)
        {
            actualCommentNumberInt++;
            actualCommentNumber.text = actualCommentNumberInt.ToString();
        }

        // Desactivate ArrowDown if we are on last comment
        if (actualCommentNumberInt == totalCommentNumberInt)
            SetArrowActive(arrowDown, false);
    }

    private void SetArrowActive(GameObject arrow, bool value)
    {
        arrow.GetComponent<Button>().enabled = value;
        arrow.GetComponent<ClickableUIVR>().enabled = value;
        arrow.transform.Find("Plane").gameObject.SetActive(value);
    }
}
