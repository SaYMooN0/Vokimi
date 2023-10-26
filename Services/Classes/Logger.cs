namespace Vokimi.Services.Classes
{
    public class Logger : Vokimi.Services.Interfaces.ILogger
    {
        private const string _logsFolder = "logs";
        private readonly string _runtimeFile, _logFile;
        public Logger()
        {
            _runtimeFile = currentDate() + "-runtime-log.txt";
            _logFile = currentDate() + "-log.txt";

            if (!Directory.Exists(_logsFolder))
                Directory.CreateDirectory(_logsFolder);

            if (!File.Exists(Path.Combine(_logsFolder, _runtimeFile)))
                File.Create(Path.Combine(_logsFolder, _runtimeFile)).Close();

            if (!File.Exists(Path.Combine(_logsFolder, _logFile)))
                File.Create(Path.Combine(_logsFolder, _logFile)).Close();

            Runtime("Instance Created");
        }
        public void CriticalError(string message) { File.AppendAllText(Path.Combine(_logsFolder, _logFile), $"[CRITICAL] {currentDate()} {message}\n"); }

        public void Error(string message) { File.AppendAllText(Path.Combine(_logsFolder, _logFile), $"[ERROR] {currentDate()} {message}\n"); }

        public void Info(string message) { File.AppendAllText(Path.Combine(_logsFolder, _logFile), $"[INFO] {currentDate()} {message}\n"); }

        public void Runtime(string message) { File.AppendAllText(Path.Combine(_logsFolder, _runtimeFile), $"{currentDate()} {message}\n"); }
        private string currentDate() => DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss");
    }
}
