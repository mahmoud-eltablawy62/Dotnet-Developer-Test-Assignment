using Dotnet_Developer_Test_Assignment.models;
using Dotnet_Developer_Test_Assignment.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Developer_Test_Assignment.Controllers
{
   
    public class CountriesController : BaseController
    {
        private readonly ICountryRepository _repository;
        private readonly ILogRepository _logRepository;
        private readonly ITemporalBlockRepository _temporalRepo;

        public CountriesController(ICountryRepository repository,
            ILogRepository logRepository,
            ITemporalBlockRepository temporalRepo)
        {
            _repository = repository;
            _logRepository = logRepository;
            _temporalRepo = temporalRepo;
        }


        [HttpPost("block")]
        public IActionResult BlockCountry([FromBody] Country country)
        {
            if (string.IsNullOrWhiteSpace(country.Code))
                return BadRequest("Country code is required");

            if (_repository.Exists(country.Code))
                return Conflict("Country already blocked");

            var added = _repository.Add(country);

            if (!added)
                return StatusCode(500, "Failed to block country");

            return Ok(country);
        }

        [HttpDelete("block/{code}")]
        public IActionResult UnblockCountry(string code)
        {
            if (!_repository.Exists(code))
                return NotFound("Country not found in blocked list");

            var removed = _repository.Remove(code);

            if (!removed)
                return StatusCode(500, "Failed to unblock country");

            return Ok(new { Message = $"{code.ToUpper()} unblocked successfully" });
        }

       
        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var countries = _repository.GetAll();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToUpper();
                countries = countries.Where(c =>
                    c.Code.ToUpper().Contains(search) ||
                    c.Name.ToUpper().Contains(search)
                );
            }

            var totalItems = countries.Count();
            var items = countries
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                page,
                pageSize,
                totalItems,
                items
            });
        }


        [HttpPost("temporal-block")]
        public IActionResult TemporalBlock([FromBody] TemporalBlock block)
        {
            if (string.IsNullOrWhiteSpace(block.CountryCode))
                return BadRequest("Country code is required.");

            block.CountryCode = block.CountryCode.ToUpper();

            if (block.Expiration == default)
                return BadRequest("Expiration must be provided.");

            if (block.Expiration < DateTime.UtcNow)
                return BadRequest("Expiration must be in the future.");

            if (block.DurationMinutes < 1 || block.DurationMinutes > 1440)
                return BadRequest("durationMinutes must be between 1 and 1440.");

            var validCountries = new HashSet<string> { "US", "GB", "EG", "FR", "DE" };
            block.CountryCode = block.CountryCode.ToUpper();

            if (!validCountries.Contains(block.CountryCode))
                return BadRequest($"Invalid country code: {block.CountryCode}");

            if (_temporalRepo.Exists(block.CountryCode))
                return Conflict($"Country {block.CountryCode} is already temporarily blocked.");


            block.Expiration = DateTime.UtcNow.AddMinutes(block.DurationMinutes);

            _temporalRepo.Add(block);


            _repository.Add(new Country
            {
                Code = block.CountryCode,
                Name = "Unknown"
            });

            return Ok(block);
        }

    }
}
