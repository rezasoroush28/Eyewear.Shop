
using Eyewear.Shop.Application.Commands.Search;
using MediatR;

namespace Eyewear.Shop.API.BackgroundServices
{
    public class IndexBuldUpHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public IndexBuldUpHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediatR = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediatR.Send(new IndexAllProductCommand());
        }
    }
}
