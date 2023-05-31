// ProjectTests.cs

using NUnit.Framework;
using TaskTracker_Modules;
using TaskTrackingApp;

namespace TaskTrackingApp.Tests
{
    public class ProjectServiceTests
    {
        private ProjectService projectService;

        [SetUp]
        public void SetUp()
        {
            // Initialize the ProjectService before each test
            projectService = new ProjectService();
        }

        [Test]
        public void AddProject_ValidProject_SuccessfullyAdded()
        {
            // Arrange
            Project project = new Project("Project 1", "Sample project");

            // Act
            projectService.AddProject(project);
            List<Project> retrievedProjects = projectService.GetProjects();

            // Assert
            Assert.Contains(project, retrievedProjects);
        }

        [Test]
        public void RemoveProject_ExistingProject_SuccessfullyRemoved()
        {
            // Arrange
            Project project = new Project("Project 1", "Sample project");
            projectService.AddProject(project);

            // Act
            bool isRemoved = projectService.RemoveProject(project);
            List<Project> retrievedProjects = projectService.GetProjects();

            // Assert
            Assert.IsTrue(isRemoved);
            Assert.IsFalse(retrievedProjects.Contains(project));
        }

        [Test]
        public void RemoveProject_NonExistingProject_ReturnsFalse()
        {
            // Arrange
            Project project = new Project("Project 1", "Sample project");

            // Act
            bool isRemoved = projectService.RemoveProject(project);

            // Assert
            Assert.IsFalse(isRemoved);
        }

        [Test]
        public void CompleteProject_ExistingProject_ProjectIsCompleted()
        {
            // Arrange
            Project project = new Project("Project 1", "Sample project");
            projectService.AddProject(project);

            // Act
            projectService.CompleteProject(project);

            // Assert
            Assert.IsTrue(project.IsCompleted);
        }

        [Test]
        public void GetProjects_NoProjects_ReturnsEmptyList()
        {
            // Act
            List<Project> retrievedProjects = projectService.GetProjects();

            // Assert
            Assert.IsEmpty(retrievedProjects);
        }
    }
}
