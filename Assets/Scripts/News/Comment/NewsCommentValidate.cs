using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Assets.Scripts.Core;

/*
 * This handles the validate button that appeares when you speak in the microphone.
 * The comment will only be add to the database if the player click on this button. 
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class NewsCommentValidate : Grabbable
    {
        
        public GameObject Buttons;
        public GameObject comment;

        // Action when you click validate, delete the two buttons
        private void ValidateAction()
        {
            // Retrieve the text of the comment
            var text = comment.GetComponent<CommentGameObject>().textOfComment;

            Database.AddComment(StaticClass.CurrentNewsId, text);
            Database.Add1CommentToPlayer();

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


        //-------------------------------------------------
        protected new void OnAttachedToHand(Hand hand)
        {
            base.OnAttachedToHand(hand);
            ValidateAction();
        }
    }
}
