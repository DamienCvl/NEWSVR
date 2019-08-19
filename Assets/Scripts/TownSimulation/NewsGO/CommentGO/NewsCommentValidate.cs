using UnityEngine;
using Assets.Scripts.Core;
using Valve.VR.InteractionSystem;

/*
 * This handles the validate button that appeares when you speak in the microphone.
 * The comment will only be add to the database if the player click on this button. 
 */

namespace Assets.Scripts.TownSimulation.NewsGO.CommentGO
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class NewsCommentValidate : ClickableUIVR
    {
        
        public GameObject Buttons;
        public GameObject comment;

        // Action when you click validate, delete the two buttons
        public void ValidateAction()
        {
            // Retrieve the text of the comment
            var text = comment.GetComponent<CommentGameObject>().textOfComment;

            Database.AddComment(StaticClass.CurrentNewsId, text);

            // Create Comment object
            Comment tmp = Database.GetLastComment();
            if (tmp.Author != null)
            {
                Comment.commentsList.Add(tmp);
                comment.GetComponent<CommentGameObject>().idComment = tmp.IdComment;
                comment.GetComponent<CommentGameObject>().FillAuthor(StaticClass.CurrentPlayerName);
                // If you just created it, it's yours so you can destroy it
                comment.GetComponent<CommentGameObject>().DeleteButton.SetActive(true);
            }
            else
            {
                comment.GetComponent<CommentGameObject>().DeleteButton.SetActive(false);
            }
            Destroy(Buttons);
            // Wait 1 second to delete the validate button to not cause bug
            Destroy(gameObject, 1);
        }
    }
}
