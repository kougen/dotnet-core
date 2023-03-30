using Infrastructure;

namespace Implementation.StandardIOManager
{
    public interface IWriter
    {
        /// <summary>
        /// This method prints out the details of the client machine and the creator of the project.
        /// </summary>
        /// <param name="githubUsername">The github username of the creator.</param>
        /// <param name="creatorName">The full name of the creator.</param>
        /// <param name="creatorId">The ID or neptun code of the creator.</param>
        void PrintSystemDetails(string githubUsername, string creatorName, string creatorId);

        /// <summary>
        /// Writes a single line with a certain severity (color), and time, while logging the console output, for later restore.
        /// </summary>
        /// <param name="severity">Tells what is the severity of the message: info, warning, error, success.</param>
        /// <param name="msg">The printed message.</param>
        void Write(MessageSeverity severity, string msg);
        void Write(string colorEscape, string msg);
        
        /// <summary>
        /// Writes a single line only, while logging the console output, for later restore.
        /// </summary>
        /// <param name="msg">The printed message.</param>
        void Write(string msg);

        /// <summary>
        /// Writes a single line with a <b>new line at the end</b> with a certain severity (color), and time, while logging the console output, for later restore.
        /// </summary>
        /// <param name="severity">Tells what is the severity of the message: info, warning, error, success.</param>
        /// <param name="msg">The printed message.</param>
        void WriteLine(MessageSeverity severity, string msg);
        void WriteLine(string colorEscape, string msg);
        
        /// <summary>
        /// Writes a single line with a <b>new line at the end</b>, while logging the console output, for later restore.
        /// </summary>
        /// <param name="msg">The printed message.</param>
        void WriteLine(string msg);
        
        void RestoreTerminalState();
    }
}
