using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using MySql.Data.MySqlClient;
using Assets.Scripts.Core;

/*
 * This handles the X button that appeares bellow comments, it only appeares if you made it.
 * Use it to delete you comment from the database.
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class NewsCommentsDeleteComments : Grabbable
    {

        public UnityEvent OnClickDelete;

        // Action when you click X, delete the Comments
        //-------------------------------------------------
        protected new void OnAttachedToHand(Hand hand)
        {
            OnClickDelete.Invoke();
        }
    }
}
