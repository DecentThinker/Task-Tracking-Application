using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_Modules
{
    public class CommentSystem
    {
        private List<string> comments;

        public CommentSystem()
        {
            comments = new List<string>();
        }

        public void AddComment(string comment)
        {
            comments.Add(comment);
        }

        public void DeleteComment(int index)
        {
            if (index >= 0 && index < comments.Count)
            {
                comments.RemoveAt(index);
            }
        }

        public void EditComment(int index, string newComment)
        {
            if (index >= 0 && index < comments.Count)
            {
                comments[index] = newComment;
            }
        }

        public List<string> GetAllComments()
        {
            return comments;
        }
    }
}
