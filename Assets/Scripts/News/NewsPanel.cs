using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using Valve.VR;

/*
 * This handle the article when you open a news item.
 * The microphone and the reaction are attached to this, so you change it's position, it moves everything.
 * You can find the srcoll function here too.
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class NewsPanel : Grabbable
    {
        [Tooltip("Action to scroll the article.")]
        public SteamVR_Action_Vector2 scrollArticleAction = SteamVR_Input.GetVector2ActionFromPath("/actions/default/in/Scroll");

        private Player player;
        // Use by NewsComment script to have the first position of the player when entered in the news
        private Transform playerFirstTransform;

        // Change this parameter to change the distance from the player of the news item, microphone and reaction box.
        public float panelDistance = 0.8f;
        // Change this parameter to change the heigth of the news item, microphone and reaction box.
        public float panelHeight = 1.3f;

        public GameObject contentNews;
        public GameObject comments;
        public GameObject oldCommentsScroll;

        private RectTransform numberToMove;
        private float whereIsTheArticle;

        // Used to see if there is a hand to release when you go out of the news and which one it is
        private Hand handToRelease;

        private void Start()
        {
            whereIsTheArticle = 0.0f;
        }

        private void OnEnable()
        {
            player = FindObjectOfType<Player>();
            playerFirstTransform = player.hmdTransform;

            // Put the news in front of the player every time the player pick up the newsSphere
            transform.position = playerFirstTransform.TransformPoint(Vector3.forward * panelDistance);
            transform.rotation = Quaternion.LookRotation(transform.position - playerFirstTransform.position, Vector3.up);

            // Set first comment position
            NewsComment.SetFirstCommentPosition(playerFirstTransform);

            oldCommentsScroll.SetActive(true);
            comments.SetActive(true);
        }

        private void OnDisable()
        {

            if(handToRelease != null)
            {
                OnDetachedFromHand(handToRelease);
            }

            oldCommentsScroll.SetActive(false);
            comments.SetActive(false);
        }

        //-------------------------------------------------
        protected new void OnAttachedToHand(Hand hand)
        {
            base.OnAttachedToHand(hand);
            handToRelease = hand;

        }


        //-------------------------------------------------
        protected new void OnDetachedFromHand(Hand hand)
        {
            base.OnDetachedFromHand(hand);
            handToRelease = null;
        }


        //-------------------------------------------------
        // Function to scroll the article
        private void ScrollArticle(Hand hand)
        {

            // The component to change to make the article scroll
            RectTransform numberToMove = (RectTransform)contentNews.GetComponent(typeof(RectTransform));

            // Find how to get the position of the thumb on the touchpad
            if (scrollArticleAction != null)
            {
                if (scrollArticleAction.GetAxis(hand.handType).y < 0)
                {
                    // Move the article from the top to the bottom by step
                    whereIsTheArticle = whereIsTheArticle + 0.003f;
                    numberToMove.offsetMax = new Vector2(numberToMove.offsetMax.x, whereIsTheArticle);
                }

                if (scrollArticleAction.GetAxis(hand.handType).y > 0)
                {
                    if (whereIsTheArticle >= 0.0f)
                    {
                        // Move the article from the bottom to the top by step
                        whereIsTheArticle = whereIsTheArticle - 0.003f;
                        numberToMove.offsetMax = new Vector2(numberToMove.offsetMax.x, whereIsTheArticle);
                    }
                }
            }
        }
    }
}
