using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation($"[HANDLE] Started handle request {typeof(TRequest)}");

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();

            var timeTaken = timer.Elapsed;

            if (timeTaken.TotalSeconds > 3)
            {
                logger.LogWarning($"[PERFORMANCE] Duration greater than 3 ({timeTaken.TotalSeconds}) seconds to handle request {typeof(TRequest)}");
            }

            logger.LogInformation($"[HANDLE] Finish handle request {typeof(TRequest)}");

            return response;
        }
    }
}
