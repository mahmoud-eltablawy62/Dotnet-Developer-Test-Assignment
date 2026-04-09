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
            var deleted = _repository.Remove(code);

            if (!deleted)
            {

                return NotFound(new { Message = $"Country with code '{code}' is not found in the blocked list." });
            }

            return Ok(new { Message = $"Country '{code}' has been unblocked successfully." });
        }



        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _repository.GetAll().AsQueryable();

            
            if (!string.IsNullOrWhiteSpace(search))
            {
              
                query = query.Where(c =>
                    c.Code.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var totalItems = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                Items = items
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
