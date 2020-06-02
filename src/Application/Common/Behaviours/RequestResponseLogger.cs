using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class RequestResponseLogger<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public RequestResponseLogger(ILogger<TRequest> logger )
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {

            var response = await next();

            _logger.LogInformation($"Name : {typeof(TRequest).Name} , Request:{request} , Response:{response}");

            return response;
        }
    }
}
