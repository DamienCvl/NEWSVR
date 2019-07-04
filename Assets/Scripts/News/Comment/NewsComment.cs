using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

/*
 * This handles the placement of the comment at it's creation.
 * Uses MicroComments to fill the text of the comment.
 */


[RequireComponent(typeof(Interactable))]
public class NewsComment : Grabbable
{
    public enum Positions { Left, Above, Right, Behind };

    public GameObject content;
    public GameObject Buttons;

    public TextMesh Author;
    [HideInInspector]
    public int id;
    public GameObject DeleteButton;

    [HideInInspector]
    public string titleOfNews;

    [HideInInspector]
    public string textOfComment;

    // Translation vector relative to the player for comments placement
    private static Vector3 commentsPosition = Vector3.left * 0.7f;
    // Position and rotation for the next comment
    public static Vector3 nextCommentPosition;
    public static Quaternion nextCommentRotation;
    // Offset vector to separate comments
    public static Vector3 commentOffset = new Vector3(-0.1f, 0, 0.1f);

    public static Vector3 firstCommentPosition;
    public static Quaternion firstCommentRotation;

    // Used to see if there is a hand to release when you go out of the news and which one it is
    private Hand handToRelease;


    ///<summary>
    ///Set the position where the comments will be placed.
    ///</summary>
    public static void SetCommentsPosition(Positions position)
    {
        switch ((int)position)
        {
            // Left
            case 0:
                commentsPosition = Vector3.left * 0.7f;
                break;
            // Above
            case 1:
                commentsPosition = Vector3.forward * 0.7f + Vector3.up * 0.4f;
                break;
            // Right
            case 2:
                commentsPosition = Vector3.right * 0.7f;
                break;
            // Behind
            case 3:
                commentsPosition = Vector3.back * 0.7f;
                break;
        }
    }

    ///<summary>
    ///Returns the starting position vector of comments relative to the player.
    ///</summary>
    public static Vector3 GetCommentsPosition()
    {
        return commentsPosition;
    }

    // Static method call in NewsPanel to set the first position of comments
    ///<summary>
    ///Set the position where the first comment will be placed relative to player first position entering the news
    ///</summary>
    public static void SetFirstCommentPosition(Transform playerFirstTransform)
    {
        // Set the transform properties where the first comment will be placed
        nextCommentPosition = playerFirstTransform.TransformPoint(commentsPosition);
        nextCommentRotation = Quaternion.LookRotation(nextCommentPosition - playerFirstTransform.position, Vector3.up);
        firstCommentPosition = nextCommentPosition;
        firstCommentRotation = nextCommentRotation;

    }

    private void Start()
    {

    }


    private void OnEnable()
    {
        // Set position and rotation of this comment
        transform.position = nextCommentPosition;
        transform.rotation = nextCommentRotation;
        // We add an offset on position for next comments
        nextCommentPosition = transform.TransformPoint(commentOffset);
    }

    private void OnDisable()
    {

        if (handToRelease != null)
        {
            OnDetachedFromHand(handToRelease);
        }

    }

    // Use in MicroComments script to fill the comment
    public void FillText(int id, string text, string title)
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

    // Called by validate button to destroy the buttons.
    public void DestroyButtons()
    {
        Destroy(Buttons);
    }

    //-------------------------------------------------
    protected new void OnAttachedToHand(Hand hand)
    {
        handToRelease = hand;
        base.OnAttachedToHand(hand);
    }


    //-------------------------------------------------
    protected new void OnDetachedFromHand(Hand hand)
    {
        handToRelease = null;
        base.OnDetachedFromHand(hand); 
    }
}
