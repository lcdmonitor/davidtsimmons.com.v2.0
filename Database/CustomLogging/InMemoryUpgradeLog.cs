using DbUp.Engine.Output;

namespace Database.CustomLogging
{
    public enum LogEntryType
    {
        Error,
        Information,
        Warning
    }

    public class LogEntry
    {
        public LogEntryType Type { get; set; }
        public String? Message { get; set; }
    }

    public class InMemoryUpgradeLog : IUpgradeLog
    {
        private List<LogEntry> _logEntries = new List<LogEntry>();


        public void WriteError(string format, params object[] args)
        {
            _logEntries.Add(new LogEntry() { Type = LogEntryType.Error, Message = string.Format(format, args) });
        }

        public void WriteInformation(string format, params object[] args)
        {
            _logEntries.Add(new LogEntry() { Type = LogEntryType.Information, Message = string.Format(format, args) });
        }

        public void WriteWarning(string format, params object[] args)
        {
            _logEntries.Add(new LogEntry() { Type = LogEntryType.Warning, Message = string.Format(format, args) });
        }

        public IEnumerable<LogEntry> GetLogEntries()
        {
            return _logEntries;
        }
    }
}