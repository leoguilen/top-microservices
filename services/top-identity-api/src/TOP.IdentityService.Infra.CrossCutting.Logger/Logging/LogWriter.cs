using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Runtime.CompilerServices;
using TOP.IdentityService.Domain.Configurations;
using TOP.IdentityService.Domain.Interfaces.Infra;

namespace TOP.IdentityService.Infra.CrossCutting.Logger.Logging
{
    public class LogWriter : ILogWriter
    {
        private readonly string _appName;
        private readonly string _elasticUri;
        private Serilog.Core.Logger _logger;

        public Guid CorrelationId { get; set; }

        public LogWriter(AppConfiguration appConfig, ElasticConfiguration elasticConfig)
        {
            _appName = appConfig.ApplicationName;
            _elasticUri = elasticConfig.Uri;
            _logger = SetupLogger();
        }

        public void Error(string message, object data = null, Exception ex = null, [CallerMemberName] string source = "")
        {
            Log(message, data, ex, CorrelationId, source, LogEventLevel.Error, _logger.Error);
        }

        public void Fatal(string message, object data = null, Exception ex = null, [CallerMemberName] string source = "")
        {
            Log(message, data, ex, CorrelationId, source, LogEventLevel.Fatal, _logger.Fatal);
        }

        public void Info(string message, object data = null, Exception ex = null, [CallerMemberName] string source = "")
        {
            Log(message, data, ex, CorrelationId, source, LogEventLevel.Information, _logger.Information);
        }

        public void Warn(string message, object data = null, Exception ex = null, [CallerMemberName] string source = "")
        {
            Log(message, data, ex, CorrelationId, source, LogEventLevel.Warning, _logger.Warning);
        }

        private Serilog.Core.Logger SetupLogger()
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}")
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(_elasticUri)) 
                {
                    AutoRegisterTemplate = true,
                })
                .CreateLogger();
        }

        private void Log(
            string message,
            object data,
            Exception ex,
            Guid? correlationId,
            string source,
            LogEventLevel level,
            Action<string, LogMessage> logger)
        {
            var logMessage = new LogMessage
            {
                Application = _appName,
                Data = data,
                Level = level.ToString(),
                Message = message,
                Method = source,
                Timestamp = DateTime.Now,
                CorrelationId = (correlationId ?? Guid.NewGuid()).ToString(),
                Error = ex
            };

            logger.Invoke("{@LogMessage}", logMessage);
        }

        ~LogWriter()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
            {
                return;
            }

            _logger?.Dispose();
            _logger = null;
            _disposed = true;
        }
    }
}
