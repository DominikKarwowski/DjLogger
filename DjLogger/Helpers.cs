using System.Diagnostics;
using System.Runtime.Versioning;

namespace DjLogger
{
    internal class Helpers
    {
        [SupportedOSPlatform("windows")]
        internal static void TryLogLoggerException(Exception ex, string message)
        {
            try
            {
                using (EventLog eventLog = new("Application.Application"))
                {
                    eventLog.WriteEvent(new EventInstance(0L, 0, EventLogEntryType.Error), $"{message}\n{ex}");
                }
            }
            catch { }
        }
    }
}
