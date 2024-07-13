using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desktop
{
    public class ErrorLogger
    {
        string path = "./helper_errors.log";
        public ErrorLogger() { }

        /// <summary>
        /// Simple task logging message to log file
        /// </summary>
        /// <param name="message">Message to be logged</param>
        /// <param name="type">Type of message</param>
        /// <param name="messageType">Optional parameter describing the name of the class of the message</param>
        /// <returns>Task</returns>
        public async Task LogMessage(string message, MessageType type, string? messageType = null) 
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path,true))
                {
                    await writer.WriteLineAsync($"{DateTime.Now.ToString()} - {type.ToString()} : {messageType} {message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SetPath(string newPath)
        {
            path = newPath;
        }
    }

    public enum MessageType
    {
        Info,
        Warning,
        Error
    }
}
