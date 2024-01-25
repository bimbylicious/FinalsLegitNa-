using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finalssho
{
    class TaskLoader
    {
        public Dictionary<string, string[]> LoadTasks(string directory)
        {
            Dictionary<string, string[]> tasks = new Dictionary<string, string[]>();
            var taskFiles = Directory.GetFiles(directory, "*.csv");

            foreach (var file in taskFiles)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    sr.ReadLine(); // Skip the header line

                    while (!sr.EndOfStream)
                    {
                        string[] values = sr.ReadLine().Split(','); // Read CSV line and split values
                        string taskName = values[0];
                        tasks.Add($"{taskName}", values);
                    }
                }
            }

            return tasks;
        }

        public Dictionary<int, string[]> LoadTasksWithNumbers(string directory)
        {
            int i = 1;
            Dictionary<int, string[]> tasks = new Dictionary<int, string[]>();
            var taskFiles = Directory.GetFiles(directory, "*.csv");

            foreach (var file in taskFiles)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    sr.ReadLine(); // Skip the header line

                    while (!sr.EndOfStream)
                    {
                        string values = sr.ReadLine();
                        tasks.Add(i, values.Split(','));
                        i++;
                    }
                }
            }

            return tasks;
        }
    }
}
