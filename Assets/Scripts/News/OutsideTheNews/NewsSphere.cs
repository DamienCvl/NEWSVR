using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;
using System;

/*
 * This scipt handels the way from inside to outside the news and all the things that includes.
 * Lot of things but htere are a lot of comments in the script.
 */

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class NewsSphere : MonoBehaviour
	{
		[EnumFlags]
		[Tooltip( "The flags used to attach this object to the hand." )]
		public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand;

		[Tooltip( "Name of the attachment transform under in the hand's hierarchy which the object should should snap to." )]
		public string attachmentPoint;

		[Tooltip( "When detaching the object, should it return to its original parent?" )]
		public bool restoreOriginalParent = false;

		private bool attached = false;

		public UnityEvent onPickUp;
		public UnityEvent onDetachFromHand;

        private GameObject HeadCollider;
        private GameObject player;

        public GameObject InTheNews;
        public GameObject NewsEnvironnement;
        private GameObject Teleport;
        public GameObject Highlight;
        public GameObject ViewNbr;
        public GameObject CommentNbr;

        private GameObject EveryNews;
        private bool thisNewsOpen;

        public Material mat;

        private bool canGoToTheHead;

        private Vector3 transformInit;

        public bool goOutWhenWalkAway;

        private void Start()
        {
            HeadCollider = GameObject.Find("HeadCollider");
            player = GameObject.Find("Player");
            Teleport = GameObject.Find("TeleportController");

            // Manage the action to open a news when we already are in one
            EveryNews = GameObject.Find("EveryNews");
            thisNewsOpen = false;

            // We keep the initial position
            transformInit = transform.position;

            // At first, the news is not open and we have to pick up the sphere to could go in there
            canGoToTheHead = false;

            // Put a magenta color to the sphere and big size
            mat.color = Color.magenta;
            transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

            // Put the view number above the sphere
            ViewNbr.GetComponent<ViewNbrView>().ReadViewNbr();

            // Put the number of comments above the sphere
            CommentNbr.GetComponent<ViewNbrComment>().ReadCommentNbr();
        }

        private void Update()
        {
            // Look at the distance between the sphere and the head of the player
            var distanceHeadBall = Vector3.Distance(transform.position, HeadCollider.transform.position);
            
            // Do things if the player take the ball to his head
            if (distanceHeadBall < 0.4f && canGoToTheHead)
            {
                // Only do this when no news is open OR if this news is open
                // Thanks to this you can't open 2 news at the same time
                if (!EveryNews.GetComponent<NewsPlacement>().aNewsIsOpen || thisNewsOpen)
                {
                    // Change the boolean to make it works
                    EveryNews.GetComponent<NewsPlacement>().aNewsIsOpen = !EveryNews.GetComponent<NewsPlacement>().aNewsIsOpen;
                    thisNewsOpen = !thisNewsOpen;

                    // We look if the news is open or not
                    if (thisNewsOpen)
                    {
                        OpenNews();
                    }
                    else
                    {
                        CloseNews();
                    }
                }
            }

            // This close the news if your head go away from the news sphere.
            if (thisNewsOpen)
            {
                if (distanceHeadBall > 2.0f && goOutWhenWalkAway)
                {
                    EveryNews.GetComponent<NewsPlacement>().aNewsIsOpen = !EveryNews.GetComponent<NewsPlacement>().aNewsIsOpen;
                    thisNewsOpen = !thisNewsOpen;
                    var tpcontrol = Teleport.GetComponent<TeleportController>();
                    tpcontrol.changeTeleport(true);
                    CloseNews();
                }
            }
        }

        // Everything happens when you take the ball to your head
        private void OpenNews ()
        {
            // Add 1 to the view number in the database
            ViewNbr.GetComponent<ViewNbrView>().Add1ViewNbr();

            // Update the view number above the sphere
            ViewNbr.GetComponent<ViewNbrView>().ReadViewNbr();

            // Free the news sphere and put it back to his initial location
            canGoToTheHead = false;

            // Active the news
            InTheNews.SetActive(true);

            // Set up the NewsEnvironnement
            NewsEnvironnement.SetActive(true);

            // Add 1 to the number of news that the current player read
            string conn = "URI=file:" + Application.dataPath + "/NewsDatabase.db"; //Path to database.
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "UPDATE PLAYER SET NBRNEWSOPEN = NBRNEWSOPEN + 1 WHERE NAME = \"" + StaticClass.CurrentPlayerName + "\";";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            // Put the sphere in green when in the news and smaller
            mat.color = Color.green;
            transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }

        // Everything happens when you take take the ball to your head when the news is open
        private void CloseNews()
        {
            // Update the number of comments above the sphere
            CommentNbr.GetComponent<ViewNbrComment>().ReadCommentNbr();

            // Free the news sphere and put it back to his initial location
            canGoToTheHead = false;

            // Put the news panel away
            InTheNews.GetComponent<FloatPlacementComments>().ForCommentsPlacement = 0f;
            InTheNews.SetActive(false);

            // Put the news environnement away
            NewsEnvironnement.SetActive(false);

            // Put the sphere back in magenta and bigger
            mat.color = Color.magenta;
            transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }


        //-------------------------------------------------
        private void OnHandHoverBegin( Hand hand )
		{
            Highlight.SetActive(true);

			// "Catch" the throwable by holding down the interaction button instead of pressing it.
			// Only do this if it isn't attached to another hand
			if ( !attached )
			{
				if ( hand.GetStandardInteractionButton() )
				{
					hand.AttachObject( gameObject, attachmentFlags, attachmentPoint );
				}
			}
		}


		//-------------------------------------------------
		private void OnHandHoverEnd( Hand hand )
		{
            Highlight.SetActive(false);

            ControllerButtonHints.HideButtonHint( hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger );
		}


		//-------------------------------------------------
		private void HandHoverUpdate( Hand hand )
		{
			//Trigger got pressed
			if (!attached && hand.GetStandardInteractionButtonDown() )
			{
				hand.AttachObject( gameObject, attachmentFlags, attachmentPoint );
				ControllerButtonHints.HideButtonHint( hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger );
            }
        }

		//-------------------------------------------------
		private void OnAttachedToHand( Hand hand )
		{
			attached = true;
            canGoToTheHead = true;

            onPickUp.Invoke();

			hand.HoverLock( null );

            // Teleport will stay inactive until we let the trigger off outside the news
            var tpcontrol = Teleport.GetComponent<TeleportController>();
            tpcontrol.changeTeleport(false);

        }


        //-------------------------------------------------
        private void OnDetachedFromHand( Hand hand )
		{
			attached = false;
            canGoToTheHead = false;

            onDetachFromHand.Invoke();

			hand.HoverUnlock( null );

            transform.position = transformInit;

            // Return the sphere to no rotation so that the title is above the sphere
            transform.rotation = new Quaternion (0, 0, 0 ,0);

            // Reactivate the teleportation if not in news and stay inactive if in it
            if (!thisNewsOpen)
            {
                var tpcontrol = Teleport.GetComponent<TeleportController>();
                tpcontrol.changeTeleport(true);
            }
		}


		//-------------------------------------------------
		private void HandAttachedUpdate( Hand hand )
		{
			//Trigger got released
			if ( !hand.GetStandardInteractionButton() )
			{
				// Detach ourselves late in the frame.
				// This is so that any vehicles the player is attached to
				// have a chance to finish updating themselves.
				// If we detach now, our position could be behind what it
				// will be at the end of the frame, and the object may appear
				// to teleport behind the hand when the player releases it.
				StartCoroutine( LateDetach( hand ) );
			}
		}


		//-------------------------------------------------
		private IEnumerator LateDetach( Hand hand )
		{
			yield return new WaitForEndOfFrame();

			hand.DetachObject( gameObject, restoreOriginalParent );
		}


		//-------------------------------------------------
		private void OnHandFocusAcquired( Hand hand )
		{
			gameObject.SetActive( true );
		}


		//-------------------------------------------------
		private void OnHandFocusLost( Hand hand )
		{
			gameObject.SetActive( false );
		}
	}
}
