using System;
using Infrastructure;
using Infrastructure.IO;
using Infrastructure.Logger;

namespace Implementation.IO
{
    public class Writer : IWriter
    {
        private readonly ILogger _logger;

        public Writer(ILogger logger)
        {
            Console.ForegroundColor = ConsoleColor.White;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void PrintSystemDetails(string githubUsername, string creatorName, string creatorId, string projectName, string desc)
        {
            var startedTime = DateTime.Now.ToString("h:mm:ss");
            var host = Environment.MachineName;
            var user = Environment.UserName;
            var platform = Environment.OSVersion.Platform;
            var message =
                $" -------- Host PC -------- \n" +
                $"[   START AT]: {startedTime}\n" +
                $"\t[     HOST]: {host}\n" +
                $"[       USER]: {user}\n" +
                $"[   PLATFORM]: {platform}\n\n\t" +
                $" -------- Creator -------- \n" +
                $"[    MADE BY]: {creatorId} ({creatorName})\n" +
                $"[     GITHUB]: https://github.com/{githubUsername}/\n\n" +
                $" -------- Details -------- \n" +
                $"[    PROJECT]: {projectName}\n" +
                $"[DESCRIPTION]: {desc}\n\n" +
                $" ------- Execution ------- \n";
            
            Write(Colorize(Constants.EscapeColors.YELLOW, message));
        }

        public void Write(MessageSeverity severity, string msg)
        {
            ConstructMsg(severity);
            Write(msg);
        }
        
        public void Write(string colorEscape, string msg)
        {
            Write(Colorize(Constants.EscapeColors.CYAN, msg));
        }

        public void Write(string msg)
        {
            Console.Write(_logger.LogWrite(msg));
        }

        public void WriteLine(MessageSeverity severity, string msg)
        {
            var prefix = ConstructMsg(severity);
            WriteLine($"{prefix}{msg}");
        }
        
        public void WriteLine(string colorEscape, string msg)
        {
            WriteLine(Colorize(colorEscape, msg));
        }

        public void WriteLine(string msg)
        {
            Console.Write(_logger.LogWriteLine(msg));
        }

        private string ConstructMsg(MessageSeverity severity)
        {
            var time = DateTime.Now.ToString("HH:mm:ss");
            switch (severity)
            {
                case MessageSeverity.Success:
                    return Colorize(Constants.EscapeColors.GREEN, $"[SUCCESS: {time}] ");
                case MessageSeverity.Info:
                    return Colorize(Constants.EscapeColors.CYAN, $"[   INFO: {time}] ");
                case MessageSeverity.Warning:
                    return Colorize(Constants.EscapeColors.YELLOW, $"[WARNING: {time}] ");
                case MessageSeverity.Error:
                    return Colorize(Constants.EscapeColors.RED, $"[  ERROR: {time}] ");
                default:
                    return Colorize(Constants.EscapeColors.RED, $"[  ERROR: {time}] Unknown Severity!\n");
            }
        }

        public void RestoreTerminalState()
        {
            var content = _logger.GetLoggedContent();
            Console.Write(content);
        }

        #region Private Methods
        private string Colorize(string colorEscape, string msg)
        {
            return $"{Constants.EscapeColors.RESET}{colorEscape}{msg}{Constants.EscapeColors.RESET}";
        }

        private string ColorizeLine(string colorEscape, string msg)
        {
            return $"{Constants.EscapeColors.RESET}{colorEscape}{msg}{Constants.EscapeColors.RESET}\n";
        }
        #endregion
    }
}
