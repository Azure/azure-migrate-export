using System;

namespace Azure.Migrate.Export.Logger
{
    public class LogEventHandler : EventArgs
    {
        public int Percentage { get; set; }
        public string Message { get; set; }

        public LogEventHandler(int percentage, string message)
        {
            Percentage = percentage;
            Message = message;
        }
    }
}