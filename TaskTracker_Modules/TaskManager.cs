using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_Modules
{
    public class TaskManager
    {
        private List<Task> tasks;

        public TaskManager()
        {
            tasks = new List<Task>();
        }
        public List<Task> getTask() { return tasks; }
        public void AddTask(Task task)
        {
            tasks.Add(task);
        }

        public void RemoveTask(Task task)
        {
            tasks.Remove(task);
        }

        public Task GetTaskById(int taskId)
        {
            return tasks.Find(t => t.Id == taskId);
        }
    }
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Task(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
