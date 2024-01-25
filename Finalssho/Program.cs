using Finalssho;
using System;
using System.Collections.Generic;
using System.IO;

namespace TaskMan
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskManagerApp taskManager = new TaskManagerApp();
            taskManager.Run();
        }
    }
}