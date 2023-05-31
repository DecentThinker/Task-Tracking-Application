using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_Modules
{
    class ViewProgress
    {
        static void Main(string[] args)
        {
            // Create a list of progress entries
            List<ProgressEntry> progressList = new List<ProgressEntry>();

            // Add some sample progress entries
            progressList.Add(new ProgressEntry { Id = 1, TaskName = "Task 1", Comment = "Completed", Time = DateTime.Now });
            progressList.Add(new ProgressEntry { Id = 2, TaskName = "Task 2", Comment = "In progress", Time = DateTime.Now });
            progressList.Add(new ProgressEntry { Id = 3, TaskName = "Task 3", Comment = "Started", Time = DateTime.Now });

            // Display progress entries
            foreach (var progress in progressList)
            {
                Console.WriteLine($"Task: {progress.TaskName}");
                Console.WriteLine($"Comment: {progress.Comment}");
                Console.WriteLine($"Time: {progress.Time}");
                Console.WriteLine();
            }
        }
    }
    public class ProgressEntry
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Comment { get; set; }
        public DateTime Time { get; set; }
    }
}
