using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finalssho
{
    class TaskManagerApp
    {
        private string directory = "tasks";
        private TaskLoader taskLoader;
        private TaskSaver taskSaver;

        public TaskManagerApp()
        {
            taskLoader = new TaskLoader();
            taskSaver = new TaskSaver();
        }

        public void Run()
        {
            PrintWelcomeMessage();

            if (!Directory.Exists(directory))
            {
                GenerateFile();
            }

            Console.Clear();

            while (true)
            {
                Dictionary<string, string[]> tasks = taskLoader.LoadTasks(directory);

                Console.WriteLine();
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("[1] Add Task");
                Console.WriteLine("[2] View Tasks");
                Console.WriteLine("[3] Update Task");
                Console.WriteLine("[4] Quit");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        AddTask();
                        break;

                    case "2":
                        Console.Clear();
                        ViewTasks(tasks);
                        break;

                    case "3":
                        Console.Clear();
                        UpdateTask(this);  // Pass the current instance to UpdateTask
                        break;

                    case "4":
                        Console.WriteLine("Goodbye...");
                        return;

                    default:
                        Console.WriteLine("Invalid Input. Please try again...");
                        break;
                }

                Console.Clear();
            }
        }

        private void GenerateFile()
        {
            Console.WriteLine($"It seems like this is the 1st time you have opened this...\n" +
                              $"Wait a moment while I generate the necessary file...");

            Directory.CreateDirectory(directory);

            Console.WriteLine("I'm done generating the necessary file\n" +
                              "Press any key to continue...");

            Console.ReadKey();
        }

        private void AddTask()
        {
            Console.WriteLine("Please enter the following information...\n");

            Console.Write("Task Name: ");
            string taskName = Console.ReadLine();

            string creationTime = DateTime.Now.ToString();

            Console.Write("Assigned To: ");
            string assignedTo = Console.ReadLine();

            string assignmentTime = string.IsNullOrEmpty(assignedTo) ? "" : DateTime.Now.ToString();

            string status = string.IsNullOrEmpty(assignedTo) ? "Open" : "Assigned";

            Console.WriteLine("\n[A] For Verification");
            Console.WriteLine("[B] Verified");
            Console.WriteLine("[C] For Revision");
            Console.WriteLine("[S] Skip");
            Console.Write("Enter the letter corresponding to the verification status: ");
            string verificationStatusInput = Console.ReadLine().Trim().ToUpper();

            string verificationStatus;
            switch (verificationStatusInput)
            {
                case "A":
                    verificationStatus = "For Verification";
                    break;
                case "B":
                    verificationStatus = "Verified" + DateTime.Now.ToString();
                    break;
                case "C":
                    verificationStatus = "For Revision" + DateTime.Now.ToString();
                    break;
                case "S":
                    verificationStatus = "";
                    break;
                default:
                    Console.WriteLine("Invalid verification status. Task not added.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
            }

            Console.Write("\nComment: ");
            string comment = Console.ReadLine();

            string[] values = { taskName, creationTime, assignedTo, assignmentTime, status, verificationStatus, comment };

            taskSaver.SaveTask(directory, values);
        }

        private void ViewTasks(Dictionary<string, string[]> tasks)
        {
            Console.WriteLine("Task List\n" +
                              "---------------------");

            int taskNumber = 1;

            foreach (var taskKey in tasks.Keys)
            {
                var values = tasks[taskKey];
                Console.WriteLine($"[{taskNumber}] Task Name: {values[0]}\n" +
                                  $"Creation Time: {values[1]}\n" +
                                  $"Assigned To: {values[2]}\n" +
                                  $"Assignment Time: {values[3]}\n" +
                                  $"Status: {values[4]}\n" +
                                  $"Verification Status: {values[5]}\n" +
                                  $"Comment: {values[6]}\n" +
                                  $"---------------------");

                taskNumber++;
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void UpdateTask(TaskManagerApp taskManagerApp)
        {
            Console.Clear();

            TaskLoader taskLoader = new TaskLoader();
            Dictionary<int, string[]> tasks = taskLoader.LoadTasksWithNumbers(directory);

            Console.WriteLine();
            Console.WriteLine("Task List");
            Console.WriteLine("---------------------");

            foreach (var key in tasks.Keys)
            {
                var values = tasks[key];
                Console.WriteLine($"[{key}] {values[0]}: {values[4]}");
            }

            Console.WriteLine("---------------------");

            Console.Write("Enter the number to update: ");
            if (!int.TryParse(Console.ReadLine(), out int selectedTaskNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Clear();

            if (tasks.ContainsKey(selectedTaskNumber))
            {
                var valuesToUpdate = tasks[selectedTaskNumber];

                Console.WriteLine($"Task Name: {valuesToUpdate[0]}\n" +
                                  $"Creation Time: {valuesToUpdate[1]}\n" +
                                  $"Assigned To: {valuesToUpdate[2]}\n" +
                                  $"Assignment Time: {valuesToUpdate[3]}\n" +
                                  $"Status: {valuesToUpdate[4]}\n" +
                                  $"Verification Status: {valuesToUpdate[5]}\n" +
                                  $"Comment: {valuesToUpdate[6]}\n" +
                                  $"---------------------");

                Console.WriteLine("What do you want to update?");

                // Available update options
                List<string> updateOptions = new List<string>();

                if (valuesToUpdate[4] == "Open")
                {
                    Console.WriteLine("[A] Assigned To");
                    updateOptions.Add("A");
                }

                if (valuesToUpdate[4].StartsWith("Close"))
                {
                    Console.WriteLine("This task is closed already...");
                }

                if (valuesToUpdate[4] == "Assigned")
                {
                    Console.WriteLine("[C] Change Status to Close");
                    updateOptions.Add("C");
                    Console.WriteLine("[B] Verification Status");
                    updateOptions.Add("B");
                }

                Console.WriteLine("[D] Quit Update");
                string updateOption = Console.ReadLine().Trim().ToUpper();

                if (!updateOptions.Contains(updateOption))
                {
                    Console.WriteLine("Invalid option. No changes made.");
                }
                else
                {
                    switch (updateOption)
                    {
                        case "A":
                            Console.Clear();
                            taskManagerApp.UpdateAssignedTo(valuesToUpdate);
                            break;

                        case "B":
                            Console.Clear();
                            taskManagerApp.UpdateVerificationStatus(valuesToUpdate);
                            break;

                        case "C":
                            Console.Clear();
                            if (valuesToUpdate[4] == "Assigned")
                            {
                                taskManagerApp.UpdateStatus(valuesToUpdate);
                            }
                            else
                            {
                                Console.WriteLine("Invalid option. No changes made.");
                            }
                            break;

                        case "D":
                            Console.WriteLine("Update canceled.");
                            break;
                    }

                    tasks[selectedTaskNumber] = valuesToUpdate;

                    // Overwrite the file
                    taskManagerApp.taskSaver.OverwriteTasks(directory, tasks);

                    Console.WriteLine($"Task '{valuesToUpdate[0]}' updated.");
                }
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        private void UpdateAssignedTo(string[] valuesToUpdate)
        {
            Console.Write("Enter new Assigned To value: ");
            string newAssignedTo = Console.ReadLine();
            valuesToUpdate[2] = newAssignedTo;
            valuesToUpdate[3] = string.IsNullOrEmpty(newAssignedTo) ? "" : DateTime.Now.ToString();
            valuesToUpdate[4] = "Assigned";
        }

        private void UpdateVerificationStatus(string[] valuesToUpdate)
        {
            Console.WriteLine("Select Verification Status:");
            Console.WriteLine("[B] Verified");
            Console.WriteLine("[C] For Revision");

            Console.Write("Enter the letter corresponding to the verification status: ");
            string newVerificationStatusInput = Console.ReadLine().Trim().ToUpper();

            switch (newVerificationStatusInput)
            {
                case "B":
                    Console.Clear();
                    string verifierDetails = "";
                    string verificationComments = "";
                    Console.Write("Input Verifier Details: ");
                    verifierDetails = Console.ReadLine();
                    Console.Write("Input Verification Comments: ");
                    verificationComments = Console.ReadLine();

                    valuesToUpdate[5] = $"Verified / {DateTime.Now} / Verifier Details: {verifierDetails} / Verification Comments: {verificationComments}";
                    UpdateStatus(valuesToUpdate);
                    break;

                case "C":
                    Console.Clear();
                    string fVerifierDetails = "";
                    string fVerificationComments = "";
                    Console.Write("Input Verifier Details: ");
                    fVerifierDetails = Console.ReadLine();
                    Console.Write("Input Verification Comments: ");
                    fVerificationComments = Console.ReadLine();

                    valuesToUpdate[5] = $"For Revision / {DateTime.Now} / Verifier Details: {fVerifierDetails} / Verification Comments: {fVerificationComments}";
                    break;

                default:
                    Console.WriteLine("Invalid verification status. No changes made.");
                    break;
            }
        }

        private void UpdateStatus(string[] valuesToUpdate)
        {
            valuesToUpdate[4] = $"Close:({DateTime.Now})";
        }

        private void PrintWelcomeMessage()
        {
            Console.WriteLine("=======================================");
            Console.WriteLine("      Welcome to the Task Manager      ");
            Console.WriteLine("=======================================");
            Console.WriteLine("This simple task manager allows you to:");
            Console.WriteLine("- Add tasks");
            Console.WriteLine("- View tasks");
            Console.WriteLine("- Update task details");
            Console.WriteLine("- Quit the application");
            Console.WriteLine("=======================================\n");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
