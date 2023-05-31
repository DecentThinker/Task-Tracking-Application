using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker_Modules;

namespace TaskTracker_Tests.Module_Tests
{
    public class ProgressEntryTests
    {
        [Test]
        public void ProgressEntry_Constructor_InitializesProperties()
        {
            // Arrange
            int id = 1;
            string taskName = "Task 1";
            string comment = "Completed";
            var time = new DateTime(2022, 1, 1);

            // Act
            var progressEntry = new ProgressEntry
            {
                Id = id,
                TaskName = taskName,
                Comment = comment,
                Time = time
            };

            // Assert
            Assert.AreEqual(id, progressEntry.Id);
            Assert.AreEqual(taskName, progressEntry.TaskName);
            Assert.AreEqual(comment, progressEntry.Comment);
            Assert.AreEqual(time, progressEntry.Time);
        }
    }
}
