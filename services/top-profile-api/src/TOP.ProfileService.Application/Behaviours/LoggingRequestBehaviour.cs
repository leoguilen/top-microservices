using MediatR;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TOP.ProfileService.Domain.Interfaces.Infra;

namespace TOP.ProfileService.Application.Behaviours
{
    public class LoggingRequestBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogWriter _logWriter;

        public LoggingRequestBehaviour(ILogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            LogRequest(request);
            var response = await next();
            LogResponse(response);

            return response;
        }

        private void LogRequest(TRequest request)
        {
            Dictionary<string, object> dicRequestProps = new();
            var requestType = request.GetType();
            
            foreach (var prop in requestType.GetProperties())
            {
                object propValue = prop.GetValue(request, null);
                dicRequestProps.Add(prop.Name, propValue);
            }

            var jsonProps = JsonSerializer.Serialize(dicRequestProps);

            _logWriter.Info($"Handling {typeof(TRequest).Name}", jsonProps);
        }

        private void LogResponse(TResponse response)
        {
            Dictionary<string, object> dicResponseProps = new();
            var responseType = response?.GetType();

            if(responseType is null)
            {
                _logWriter.Info($"Handled {typeof(TResponse).Name}");
                return;
            }
            
            foreach (var prop in responseType.GetProperties())
            {
                object propValue = prop.GetValue(response, null);
                dicResponseProps.Add(prop.Name, propValue);
            }

            var jsonProps = JsonSerializer.Serialize(dicResponseProps);

            _logWriter.Info($"Handled {typeof(TResponse).Name}", jsonProps);
        }
    }
}
