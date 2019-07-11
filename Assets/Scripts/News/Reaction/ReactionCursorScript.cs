using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using Assets.Scripts.Core;

/*
 * Handles the reaction box.
 * This handles the white ball and it's placement on the box.
 * Depending of it's position when you leave the news, it will add one to reaction wanted.
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class ReactionCursorScript : MonoBehaviour
    {
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand;

        [EnumFlags]
        [Tooltip("Name of the attachment transform under in the hand's hierarchy which the object should should snap to.")]
        public GrabTypes grabTypes = GrabTypes.Grip;

        [Tooltip("Action to interact with UI.")]
        public SteamVR_Action_Boolean UI_Interaction_Action = SteamVR_Input.GetBooleanActionFromPath("/actions/default/in/InteractUI");

        [Tooltip("When detaching the object, should it return to its original parent?")]
        public bool restoreOriginalParent = false;

        private bool attached = false;

        public GameObject Highlight;
        public Text title;
       

        private Vector3 scale;
        private Quaternion rotation;

        private void Start()
        {
            // Resolve the bug that the cursor will grow when release in vr (don't know where that come from)
            // Not really resolve but a bit better.
            scale = this.gameObject.transform.localScale;
            rotation = this.gameObject.transform.rotation;
        }

        private void OnEnable()
        {
            transform.localPosition = new Vector3(-0.1f, 0, 0);
        }

        

        private void AddReactionToDatabasePlayer(string reactionType)
        {
            // Add 1 to the reaction wanted in the database for the player
            string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "UPDATE PLAYER SET " + reactionType + " = " + reactionType + " + 1 WHERE NAME = \"" + StaticClass.CurrentPlayerName + "\" ";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }

        private void OnDisable()
        {
            if (transform.localPosition.y < 0f)
            {
                if(transform.localPosition.z < 0f)
                {
                    // Add 1 to Sad in NEWS table and PLAYER table
                    Database.AddReactionToDatabaseNews("Sad");
                    AddReactionToDatabasePlayer("NBRSAD");
                }
                else
                {
                    // Add 1 to Angry in NEWS table and PLAYER table
                    AddReactionToDatabaseNews("ANGRYNBR");
                    AddReactionToDatabasePlayer("NBRANGRY");
                }
            }

            if (transform.localPosition.y > 0f)
            {
                if (transform.localPosition.z < 0f)
                {
                    // Add 1 to Happy in NEWS table and PLAYER table
                    AddReactionToDatabaseNews("HAPPYNBR");
                    AddReactionToDatabasePlayer("NBRHAPPY");
                }
                else
                {
                    // Add 1 to Surprise in NEWS table and PLAYER table
                    AddReactionToDatabaseNews("SURPRISENBR");
                    AddReactionToDatabasePlayer("NBRSURPRISE");
                }
            }
        }

        //-------------------------------------------------
        private void OnHandHoverBegin(Hand hand)
        {
            Highlight.SetActive(true);

            // "Catch" the throwable by holding down the interaction button instead of pressing it.
            // Only do this if it isn't attached to another hand
            if (!attached)
            {
                if (UI_Interaction_Action.GetStateDown(hand.handType))
                {
                    hand.AttachObject(gameObject, grabTypes, attachmentFlags);
                }
            }
        }


        //-------------------------------------------------
        private void OnHandHoverEnd(Hand hand)
        {
            Highlight.SetActive(false);

            ControllerButtonHints.HideButtonHint(hand, UI_Interaction_Action);
        }


        //-------------------------------------------------
        private void HandHoverUpdate(Hand hand)
        {
            //Trigger got pressed
            if (!attached && UI_Interaction_Action.GetStateDown(hand.handType))
            {
                hand.AttachObject(gameObject, grabTypes, attachmentFlags);
                ControllerButtonHints.HideButtonHint(hand, UI_Interaction_Action);
            }
        }

        //-------------------------------------------------
        private void OnAttachedToHand(Hand hand)
        {
            attached = true;

            hand.HoverLock(null);

        }


        //-------------------------------------------------
        private void OnDetachedFromHand(Hand hand)
        {
            attached = false;

            hand.HoverUnlock(null);

            Vector3 pos;

            // Put the ball at the middle of the reaction wanted.
            if (transform.localPosition.y < 0f)
            {
                if (transform.localPosition.z < 0f)
                {
                    // Sad
                    pos = new Vector3(0, -0.25f, -0.25f);
                }
                else
                {
                    // Angry
                    pos = new Vector3(0, -0.25f, 0.25f);
                }
            }
            else
            {
                if (transform.localPosition.z < 0f)
                {
                    // Happy
                    pos = new Vector3(0, 0.25f, -0.25f);
                }
                else
                {
                    // Surprise
                    pos = new Vector3(0, 0.25f, 0.25f);
                }
            }

            // So that the player can put the ball back at the middle if he doesn't want to leave a reaction.
            if (transform.localPosition.y >-0.05f && transform.localPosition.y < 0.05f && transform.localPosition.z > -0.05f && transform.localPosition.z < 0.05f)
            {
                pos = new Vector3(-0.1f,0,0);
            }

            transform.localPosition = pos;

            // Fot the deformation bug but doesn't really do the job.
            this.gameObject.transform.localScale = scale;
            this.gameObject.transform.rotation = rotation;
        }


        //-------------------------------------------------
        private void HandAttachedUpdate(Hand hand)
        {
            //Trigger got released
            if (UI_Interaction_Action.GetStateUp(hand.handType))
            {
                // Detach ourselves late in the frame.
                // This is so that any vehicles the player is attached to
                // have a chance to finish updating themselves.
                // If we detach now, our position could be behind what it
                // will be at the end of the frame, and the object may appear
                // to teleport behind the hand when the player releases it.
                StartCoroutine(LateDetach(hand));
            }
        }


        //-------------------------------------------------
        private IEnumerator LateDetach(Hand hand)
        {
            yield return new WaitForEndOfFrame();

            hand.DetachObject(gameObject, restoreOriginalParent);
        }


        //-------------------------------------------------
        private void OnHandFocusAcquired(Hand hand)
        {
            gameObject.SetActive(true);
        }


        //-------------------------------------------------
        private void OnHandFocusLost(Hand hand)
        {
            gameObject.SetActive(false);
        }
    }
}
