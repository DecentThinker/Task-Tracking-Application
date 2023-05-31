using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_Modules
{
    public class Project
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public Project(string name, string description)
        {
            Name = name;
            Description = description;
            IsCompleted = false;
        }
    }
    public class ProjectService
    {
        private List<Project> projects;

        public ProjectService()
        {
            // Initialize the project list
            projects = new List<Project>();
        }

        public void AddProject(Project project)
        {
            projects.Add(project);
        }

        public bool RemoveProject(Project project)
        {
            return projects.Remove(project);
        }

        public void CompleteProject(Project project)
        {
            project.IsCompleted = true;
        }

        public List<Project> GetProjects()
        {
            return projects;
        }
    }
}
