using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Assets.Scripts.Core;

/*
 * This scipt handels the way from inside to outside the news and all the things that includes.
 * Lot of things but there are a lot of comments in the script.
 */

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class NewsSphere : Grabbable
	{

        private GameObject HeadCollider;
        private GameObject player;

        public NewsGameObject news;
        public GameObject InTheNews;
        public GameObject NewsEnvironnement;
        private GameObject Teleport;
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
            CommentNbr.GetComponent<ViewNbrComment>().DisplayCommentNbr();
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
            // Set the id of the news opened in StaticClass
            StaticClass.CurrentNewsId = news.Id;

            // Desactivate beacon linked to the news if it is activated
            if (StaticClass.newsBeaconedList.Contains(news.Id))
            {
                StaticClass.newsBeaconedList.Remove(news.Id);
                news.beacon.SetActive(false);
            }

            Database.ViewCountApproval();

            // Update the view number above the sphere
            ViewNbr.GetComponent<ViewNbrView>().ReadViewNbr();

            // Free the news sphere and put it back to his initial location
            canGoToTheHead = false;

            // Active the news. Trigger OnEnable in NewsPanel which activate comments and many stuff.
            InTheNews.SetActive(true);

            // Set up the NewsEnvironnement
            NewsEnvironnement.SetActive(true);

            // Put the sphere in green when in the news and smaller
            mat.color = Color.green;
            transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }

        // Everything happens when you take take the ball to your head when the news is open
        private void CloseNews()
        {
            // Update the number of comments above the sphere
            CommentNbr.GetComponent<ViewNbrComment>().DisplayCommentNbr();

            // Free the news sphere and put it back to his initial location
            canGoToTheHead = false;

            // Put the news panel away
            InTheNews.SetActive(false);

            // Put the news environnement away
            NewsEnvironnement.SetActive(false);

            // Put the sphere back in magenta and bigger
            mat.color = Color.magenta;
            transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }

		//-------------------------------------------------
		protected new void OnAttachedToHand( Hand hand )
		{
            base.OnAttachedToHand(hand);
            canGoToTheHead = true;
            // Teleport will stay inactive until we let the trigger off outside the news
            var tpcontrol = Teleport.GetComponent<TeleportController>();
            tpcontrol.changeTeleport(false);

            // Detach any object in the other hand when we grab the news sphere
            hand.otherHand.DetachObject(hand.otherHand.currentAttachedObject);

            // Disable other hand to not grab anything
            hand.otherHand.enabled = false;
        }


        //-------------------------------------------------
        protected new void OnDetachedFromHand( Hand hand )
		{
            base.OnDetachedFromHand(hand);
            canGoToTheHead = false;
            transform.position = transformInit;

            // Return the sphere to no rotation so that the title is above the sphere
            transform.rotation = new Quaternion (0, 0, 0 ,0);

            // Reactivate the teleportation if not in news and stay inactive if in it
            if (!thisNewsOpen)
            {
                var tpcontrol = Teleport.GetComponent<TeleportController>();
                tpcontrol.changeTeleport(true);
            }

            hand.otherHand.enabled = true;
        }
	}
}
