using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Service
{
    internal interface ILogger
    {
        void Log(string message, string level);
        void Log(string message, string level, string className, string methodName);
    }
}
