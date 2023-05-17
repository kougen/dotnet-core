namespace Infrastructure.IO
{
    public interface IWriter
    {
        /// <summary>
        /// This method prints out the details of the client machine and the creator of the project.
        /// </summary>
        /// <param name="githubUsername">The github username of the creator.</param>
        /// <param name="creatorName">The full name of the creator.</param>
        /// <param name="creatorId">The ID or neptun code of the creator.</param>
        /// <param name="projectName">The name of the project that being run.</param>
        /// <param name="desc">The long descriptions of the project.</param>
        void PrintSystemDetails(string githubUsername, string creatorName, string creatorId, string projectName, string desc);

        /// <summary>
        /// Writes a single line with a certain severity (color), and time, while logging the console output, for later restore.
        /// </summary>
        /// <param name="severity">Tells what is the severity of the message: info, warning, error, success.</param>
        /// <param name="msg">The printed message.</param>
        void Write(MessageSeverity severity, string msg);
        
        /// <summary>
        /// Writes a single line with a certain color, while logging the console output, for later restore.
        /// </summary>
        /// <param name="colorEscape">The color escape. Suggested using the escapes from the Constants class</param>
        /// <param name="msg"></param>
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
        
        /// <summary>
        /// Writes a single line with a new line at the end in a certain color, while logging the console output, for later restore.
        /// </summary>
        /// <param name="colorEscape">The color escape. Suggested using the escapes from the Constants class</param>
        /// <param name="msg"></param>
        void WriteLine(string colorEscape, string msg);
        
        /// <summary>
        /// Writes a single line with a <b>new line at the end</b>, while logging the console output, for later restore.
        /// </summary>
        /// <param name="msg">The printed message.</param>
        void WriteLine(string msg);
        
        /// <summary>
        /// Restores the logged terminal from the file.
        /// </summary>
        void RestoreTerminalState();
    }
}
