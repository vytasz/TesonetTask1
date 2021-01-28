using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TesonetTask1.Modules
{
    public class Logger //should improve with DI or scrap and use premade library
    {
        private static Logger _instance;

        protected Logger()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("EventLog.log", "myListener"));
        }

        public static Logger GetInstance()//singleton pattern
        {
            if (_instance == null)
            {
                _instance = new Logger();
            }

            return _instance;
        }

        private void Log(string message)
        {
            DateTimeOffset time = DateTimeOffset.Now;
            Trace.TraceInformation(time + ": " + message);
            Trace.Flush();
        }

        public void Error(Exception e)
        {
            var message = e.Message;
            Log(message);
        }

        internal void Unauthorized(string location)
        {
            var message = "Unauthorized access: " + location;
            Log(message);
        }
    }
}
