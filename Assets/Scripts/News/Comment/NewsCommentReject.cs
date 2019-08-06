using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

/*
 * This handles the reject button.
 * You can use it just after you spoke in the microphone, since you don't click the validate button (and so not add the comment to the database) you only have to delete the comment in the scene.
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class NewsCommentReject : ClickableUIVR
    {

        public GameObject Comment;

        // Action when you click the delete button, delete all the comment
        public void RejectAction()
        {
            Destroy(Comment);
            // Wait 1 second to delete the reject button to not cause bug
            Destroy(gameObject, 1);
        }
    }
}
