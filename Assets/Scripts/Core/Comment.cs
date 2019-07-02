using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core
{
    class Comment
    {
        private uint idComment;
        private DateTime date;
        private string content;
        private string author;

        public Comment(uint idComment, DateTime date, string content, string author)
        {
            this.idComment = idComment;
            this.date = date;
            this.content = content;
            this.author = author;
        }
    }
}
