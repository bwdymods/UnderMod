using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderMod.Utils
{
    public class Logger : UnderModAPI.Utils.ILogger
    {
        private FileStream LogFileStream;
        private StreamWriter LogFileStreamWriter;
        private int LogLevel = 0;

        public Logger()
        {
            LogFileStream = new FileStream("UnderMod-Log.txt", FileMode.Create);
            LogFileStreamWriter = new StreamWriter(LogFileStream);
            LogFileStreamWriter.AutoFlush = true;
            string h = "\n                                   _  _  __ _  ____  ____  ____  _  _   __  ____ \n                                  / )( \\(  ( \\(    \\(  __)(  _ \\( \\/ ) /  \\(    \\\n                                  ) \\/ (/    / ) D ( ) _)  )   // \\/ \\(  O )) D (\n                                  \\____/\\_)__)(____/(____)(__\\_)\\_)(_/ \\__/(____/\n\n";
            h += "                   UnderMod " + API.instance.GetAPIVersion().ToString() + " : : Mod loader for UnderMine " + UnityEngine.Application.version + " : : created by bwdymods\n\n";
            Log(h);
        }

        /// <summary>
        /// Log level 0: Used to trace the entry and exit of functions for intensive debugging.
        /// </summary>
        /// <param name="message"></param>
        public void Trace(string message)
        {
            message = Timestamp + "[TRACE] " + message;
            if (LogLevel <= 0)
            {
                var origc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(message);
                Console.ForegroundColor = origc;
            }
            LogFileStreamWriter.WriteLine(message);
        }

        /// <summary>
        /// Log level 1: Used to provide general information that may be useful to the developer.
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            message = Timestamp + "[DEBUG] " + message;
            if (LogLevel <= 1)
            {
                var origc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(message);
                Console.ForegroundColor = origc;
            }
            LogFileStreamWriter.WriteLine(message);
        }

        /// <summary>
        /// Log level 2: Used to provide general information that may be useful to the end-user.
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            message = Timestamp + "[INFO] " + message;
            if (LogLevel <= 2)
            {
                var origc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(message);
                Console.ForegroundColor = origc;
            }
            LogFileStreamWriter.WriteLine(message);
        }

        /// <summary>
        /// Log level 3: Used to document that a problem exists, but will not interfere with the game functionality.
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message)
        {
            message = Timestamp + "[WARN] " + message;
            if (LogLevel <= 3)
            {
                var origc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message);
                Console.ForegroundColor = origc;
            }
            LogFileStreamWriter.WriteLine(message);
        }

        /// <summary>
        /// Log level 4: Used to document that a problem exists, which will possibly interfere with the game functionality.
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            message = Timestamp + "[ERROR] " + message;
            if (LogLevel <= 4)
            {
                var origc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = origc;
            }
            LogFileStreamWriter.WriteLine(message);
        }

        /// <summary>
        /// Log level 5: Use sparingly to make the end-user aware of action items that need their involvement, e.g. updates.
        /// </summary>
        /// <param name="message"></param>
        public void Alert(string message)
        {
            message = Timestamp + "[ALERT] " + message;
            if (LogLevel <= 5)
            {
                var origc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(message);
                Console.ForegroundColor = origc;
            }
            LogFileStreamWriter.WriteLine(message);
        }

        /// <summary>
        /// Log level 6: Used to document that a problem exists, which will likely prevent the game from continuing.
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(string message)
        {
            message = Timestamp + "[FATAL] " + message;
            if(LogLevel <= 6)
            {
                var origc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(message);
                Console.ForegroundColor = origc;
            }
            LogFileStreamWriter.WriteLine(message);
        }

        private string Timestamp { get { return "[" + DateTime.Now.ToString("s") + "]"; } }

        private void Log(string msg)
        {
            Console.WriteLine(msg);
            LogFileStreamWriter.WriteLine(msg);
        }
    }
}
