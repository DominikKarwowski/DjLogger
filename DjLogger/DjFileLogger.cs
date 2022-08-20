using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjLogger
{
    public class FileLogger<TCategoryName> : ILogger<TCategoryName>
    {
        private readonly string _logFilePath;
        private string Name => typeof(TCategoryName).ToString();
        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) =>
            logLevel == LogLevel.Information
            || logLevel == LogLevel.Warning
            || logLevel == LogLevel.Error;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            if (formatter is null) formatter = DefaultFormatter<TState>();

            var value = formatter(state, exception);
            if (string.IsNullOrEmpty(value)) return;

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var logLine = $"{timestamp} - Category: {Name} - EventId: {eventId.Id} - {value}";

            try
            {
                var path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    AppDomain.CurrentDomain.FriendlyName + ".log");
                    //_logFilePath);
                File.AppendAllText(path, logLine);
            }
            catch (Exception)
            {

            }
        }


        private Func<TState, Exception?, string> DefaultFormatter<TState>() =>
            (s, ex) =>
            {
                string? result;
                if (ex == null) result = s?.ToString();
                else result = s?.ToString() + Environment.NewLine + ex.Message;
                return result == null ? string.Empty : result;
            };

    }
}
