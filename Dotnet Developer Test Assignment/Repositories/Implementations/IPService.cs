using Dotnet_Developer_Test_Assignment.models;
using Dotnet_Developer_Test_Assignment.Repositories.Interfaces;
using System.Text.Json;

namespace Dotnet_Developer_Test_Assignment.Repositories.Implementations
{
    public class IPService : IIPService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public IPService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<IpInfo?> LookupIpAsync(string? ipAddress = null)
        {
            var client = _httpClientFactory.CreateClient();

            var apiKey = _config["IpApi:ApiKey"];
            var baseUrl = _config["IpApi:BaseUrl"];

            
            var url = string.IsNullOrWhiteSpace(ipAddress)
                ? $"{baseUrl}?apiKey={apiKey}"              
                : $"{baseUrl}?apiKey={apiKey}&ip={ipAddress}";  

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                Console.WriteLine("IP API Error: " + text);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            return new IpInfo
            {
                IpAddress = doc.RootElement.GetProperty("ip").GetString() ?? "",
                CountryCode = doc.RootElement.GetProperty("country_code2").GetString() ?? "",
                CountryName = doc.RootElement.GetProperty("country_name").GetString() ?? "",
                Isp = doc.RootElement.GetProperty("isp").GetString() ?? ""
            };
        }
    }
}