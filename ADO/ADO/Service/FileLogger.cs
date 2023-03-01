using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Service
{
    internal class FileLogger : ILogger
    {
        private readonly string fileName;
        public FileLogger(string fileName)
        {
            this.fileName = "logs.txt";
        }
        
        public void Log(string message, string level)
        {
            this.Log(message, level, "", "");
        }

        public void Log(string message, string level, string className, string methodName)
        {
            File.AppendAllText(fileName,$"{DateTime.Now} | {level} | {message} | {className}.{methodName}\n");
        }
    }
}
