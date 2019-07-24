using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Comment
    {
        public uint IdComment { get; private set; }
        public DateTime Date { get; private set; }
        public string Content { get; private set; }
        public string Author { get; private set; }

        private readonly GameObject commentPreFab = (GameObject)Resources.Load("Prefabs/News/Comment", typeof(GameObject));

        public static List<Comment> commentsList = new List<Comment>();

        //null comment
        public Comment(){ this.Author = null; }

        public Comment(uint idComment, DateTime date, string content, string author)
        {
            this.IdComment = idComment;
            this.Date = date;
            this.Content = content;
            this.Author = author;
        }

        public void GenerateGameObject(Transform commentParent)
        {
            GameObject comment = UnityEngine.Object.Instantiate(commentPreFab, commentParent);
            CommentGameObject cmtGO = comment.GetComponent<CommentGameObject>();
            cmtGO.FillText(Content);
            cmtGO.FillAuthor(Author);
            cmtGO.idComment = IdComment;
            cmtGO.DestroyButtons();

            // Add the delete comments option if the current player is the one who made the comments before.
            if (Author == StaticClass.CurrentPlayerName)
            {
                comment.GetComponent<CommentGameObject>().DeleteButton.SetActive(true);
            }

            cmtGO.PlaceComment();
        }

        public void Delete()
        {
            Database.DeleteComment(IdComment);
            commentsList.Remove(this);
        }
    }
}
