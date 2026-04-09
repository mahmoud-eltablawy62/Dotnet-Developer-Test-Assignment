using Dotnet_Developer_Test_Assignment.Repositories.Interfaces;

namespace Dotnet_Developer_Test_Assignment.Repositories.Implementations
{
    public class TemporalBlockCleanupService
    : BackgroundService
    {
        private readonly ITemporalBlockRepository _temporalRepo;
        private readonly ICountryRepository _countryRepo;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public TemporalBlockCleanupService(
            ITemporalBlockRepository temporalRepo,
            ICountryRepository countryRepo)
        {
            _temporalRepo = temporalRepo;
            _countryRepo = countryRepo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var expired = _temporalRepo.GetAll()
                    .Where(b => b.Expiration <= now)
                    .ToList();

                foreach (var block in expired)
                {
                    _temporalRepo.Remove(block.CountryCode);
                    _countryRepo.Remove(block.CountryCode); 
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}