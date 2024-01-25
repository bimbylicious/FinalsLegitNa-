using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finalssho
{
    class TaskSaver
    {
        public void SaveTask(string directory, string[] values)
        {
            string taskName = values[0];
            string filePath = Path.Combine(directory, "tasks.csv");

            if (!File.Exists(filePath))
            {
                using (StreamWriter headerWriter = new StreamWriter(filePath))
                {
                    headerWriter.WriteLine("TaskName,CreationTime,AssignedTo,AssignmentTime,Status,VerificationStatus,Comment");
                }
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(string.Join(",", values));
            }

            Console.WriteLine($"Task '{taskName}' added to the task list.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void OverwriteTasks(string directory, Dictionary<int, string[]> tasks)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(directory, "tasks.csv")))
            {
                sw.WriteLine("TaskName,CreationTime,AssignedTo,AssignmentTime,Status,VerificationStatus,Comment");

                foreach (var task in tasks.Values)
                {
                    sw.WriteLine(string.Join(",", task));
                }
            }
        }
    }
}
