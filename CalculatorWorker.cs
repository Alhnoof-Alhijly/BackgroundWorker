using Template.Database;
using Template.Services.Calculator;

namespace BackgroundWorker
{
    public class CalculatorWorker : BackgroundService
    {
        private readonly ILogger<CalculatorWorker> _logger;
        private IServiceScopeFactory _serviceScopeFactory;
        private DatabaseContext _databaseContext;
        private ICalculatorService _calculatorService;
        public CalculatorWorker(
            ILogger<CalculatorWorker> logger,
            IServiceScopeFactory serviceScopeFactory
            )
        {

            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () => await Calculate(stoppingToken), stoppingToken);
        }

        private async Task Calculate(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var scope = _serviceScopeFactory.CreateScope();
                _databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                _calculatorService = scope.ServiceProvider.GetRequiredService<ICalculatorService>();
                while (1 == 1)
                {

                    _logger.LogInformation("Worker for calculator is starting now");
                    await _calculatorService.CalculatePoint();
                    _logger.LogInformation("Now worker will wait for a day");
                    await Task.Delay(TimeSpan.FromDays(1));
                    //await Task.Delay(1000);
                }
            }
        }
    }
}
