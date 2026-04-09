using Dotnet_Developer_Test_Assignment.models;
using Dotnet_Developer_Test_Assignment.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dotnet_Developer_Test_Assignment.Controllers
{
    public class IPController : BaseController
    {
        private readonly IIPService _ipService;
        private readonly ICountryRepository _countryRepo;
        private readonly ILogRepository _logRepo;

        public IPController(IIPService ipService, ICountryRepository countryRepo, ILogRepository logRepo)
        {
            _ipService = ipService;
            _countryRepo = countryRepo;
            _logRepo = logRepo;
        }


        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress)
        {
            
            var ipInfo = await _ipService.LookupIpAsync(ipAddress);

            if (ipInfo == null)
                return StatusCode(500, "Failed to lookup IP");

            return Ok(ipInfo);
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock([FromQuery] string? ipAddress)
        {

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            }

         
            var ipInfo = await _ipService.LookupIpAsync(ipAddress);
            if (ipInfo == null)
                return StatusCode(500, "Failed to lookup IP");

            
            bool isBlocked = _countryRepo.Exists(ipInfo.CountryCode);

           
            var log = new BlockedAttemptLog
            {
                IpAddress = ipInfo.IpAddress,
                CountryCode = ipInfo.CountryCode,
                IsBlocked = isBlocked,
                UserAgent = Request.Headers["User-Agent"].ToString(),
                Timestamp = DateTime.UtcNow
            };
            _logRepo.Add(log);


            return Ok(new
            {
                ipInfo.IpAddress,
                ipInfo.CountryCode,
                ipInfo.CountryName,
                isBlocked
            });
        }


        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts(
       [FromQuery] int page = 1,
       [FromQuery] int pageSize = 10)
        {
            var allLogs = _logRepo.GetAll().OrderByDescending(l => l.Timestamp);

            var totalItems = allLogs.Count();
            var items = allLogs
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
    }
}
