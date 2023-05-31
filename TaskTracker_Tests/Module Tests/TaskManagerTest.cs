using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker_Modules;
using Task = TaskTracker_Modules.Task;

namespace TaskTracker_Tests.Module_Tests
{
     public class TaskManagerTests
    {
        [Test]
        public void TaskManager_AddTask_AddsTaskToList()
        {
            // Arrange
            var taskManager = new TaskManager();
            var task = new Task(1, "Task 1", "Description");

            // Act
            taskManager.AddTask(task);

            // Assert
            Assert.Contains(task, taskManager.getTask());
        }

        [Test]
        public void TaskManager_RemoveTask_RemovesTaskFromList()
        {
            // Arrange
            var taskManager = new TaskManager();
            var task = new Task(1, "Task 1", "Description");
            taskManager.AddTask(task);

            // Act
            taskManager.RemoveTask(task);

            // Assert
            Assert.IsFalse(taskManager.getTask().Contains(task));
        }

        [Test]
        public void TaskManager_GetTaskById_ReturnsCorrectTask()
        {
            // Arrange
            var taskManager = new TaskManager();
            var task1 = new Task(1, "Task 1", "Description 1");
            var task2 = new Task(2, "Task 2", "Description 2");
            taskManager.AddTask(task1);
            taskManager.AddTask(task2);

            // Act
            var foundTask = taskManager.GetTaskById(2);

            // Assert
            Assert.AreEqual(task2, foundTask);
        }
    }
}
