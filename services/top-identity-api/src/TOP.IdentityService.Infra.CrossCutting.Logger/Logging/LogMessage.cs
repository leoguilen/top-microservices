using System;

namespace TOP.IdentityService.Infra.CrossCutting.Logger.Logging
{
    public class LogMessage
    {
        public DateTime Timestamp { get; set; }
        public string Application { get; set; }
        public string Level { get; set; }
        public string Method { get; set; }
        public string Message { get; set; }
        public string CorrelationId { get; set; }
        public object Data { get; set; }
        public Exception Error { get; set; }
    }
}
