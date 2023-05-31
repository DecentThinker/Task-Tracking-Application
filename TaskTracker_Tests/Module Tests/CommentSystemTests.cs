using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker_Modules;

namespace TaskTracker_Tests.Module_Tests
{
    public class CommentSystemTests
    {
        [Test]
        public void AddComment_ShouldAddCommentToList()
        {
            // Arrange
            var commentSystem = new CommentSystem();
            var comment = "This is a comment";

            // Act
            commentSystem.AddComment(comment);

            // Assert
            Assert.AreEqual(1, commentSystem.GetAllComments().Count);
            Assert.AreEqual(comment, commentSystem.GetAllComments()[0]);
        }

        [Test]
        public void DeleteComment_ShouldDeleteCommentFromList()
        {
            // Arrange
            var commentSystem = new CommentSystem();
            var comment1 = "Comment 1";
            var comment2 = "Comment 2";
            commentSystem.AddComment(comment1);
            commentSystem.AddComment(comment2);

            // Act
            commentSystem.DeleteComment(0);

            // Assert
            Assert.AreEqual(1, commentSystem.GetAllComments().Count);
            Assert.AreEqual(comment2, commentSystem.GetAllComments()[0]);
        }

        [Test]
        public void EditComment_ShouldEditCommentInList()
        {
            // Arrange
            var commentSystem = new CommentSystem();
            var comment = "This is a comment";
            commentSystem.AddComment(comment);

            // Act
            commentSystem.EditComment(0, "Edited comment");

            // Assert
            Assert.AreEqual(1, commentSystem.GetAllComments().Count);
            Assert.AreEqual("Edited comment", commentSystem.GetAllComments()[0]);
        }
    }
}
