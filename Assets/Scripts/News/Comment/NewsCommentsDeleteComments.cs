using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using MySql.Data.MySqlClient;

/*
 * This handles the X button that appeares bellow comments, it only appeares if you made it.
 * Use it to delete you comment from the database.
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class NewsCommentsDeleteComments : Connection
    {

        public GameObject Comment;

        // Action when you click X, delete the Comments
        private void DeleteAction()
        {
            ConnectDB();
            MySqlCommand cmdDeleteAction = new MySqlCommand("DELETE FROM COMMENTS WHERE ID = @dbIdComment;", con);
            cmdDeleteAction.Parameters.AddWithValue("@dbIdComment", Comment.GetComponent<NewsComment>().id);

            try
            {
                cmdDeleteAction.ExecuteReader();
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
            }

            cmdDeleteAction.Dispose();
            con.Dispose();

            // Wait 1 second to delete the comment button to not cause bug
            Destroy(Comment, 1);
            Destroy(gameObject,1);
        }

        //-------------------------------------------------
        protected void OnAttachedToHand(Hand hand)
        {
            DeleteAction();
        }
    }
}
