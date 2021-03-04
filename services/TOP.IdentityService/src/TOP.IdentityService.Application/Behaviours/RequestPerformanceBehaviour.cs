using MediatR;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Interfaces.Infra;

namespace TOP.IdentityService.Application.Behaviours
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogWriter _logWriter;

        public RequestPerformanceBehaviour(ILogWriter logWriter)
        {
            _timer = new Stopwatch();
            _logWriter = logWriter;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            if (_timer.ElapsedMilliseconds > 500)
            {
                var name = typeof(TRequest).Name;

                _logWriter.Warn($"Long Running Request: {name} ({_timer.ElapsedMilliseconds} milliseconds) {request}");
            }

            return response;
        }
    }
}
