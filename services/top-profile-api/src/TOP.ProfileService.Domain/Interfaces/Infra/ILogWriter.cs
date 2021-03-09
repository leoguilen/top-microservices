using System;
using System.Runtime.CompilerServices;

namespace TOP.ProfileService.Domain.Interfaces.Infra
{
    public interface ILogWriter : IDisposable
    {
        public Guid CorrelationId { get; set; }

        void Info(string message, object data = default, Exception ex = default, 
            [CallerMemberName] string source = "");

        void Warn(string message, object data = default, Exception ex = default, 
            [CallerMemberName] string source = "");

        void Error(string message, object data = default, Exception ex = default, 
            [CallerMemberName] string source = "");
        
        void Fatal(string message, object data = default, Exception ex = default, 
            [CallerMemberName] string source = "");
    }
}
