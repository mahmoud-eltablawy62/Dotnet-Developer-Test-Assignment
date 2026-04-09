namespace Dotnet_Developer_Test_Assignment.models
{
    public class TemporalBlock 
    {
        public string CountryCode { get; set; } = string.Empty;

        public int DurationMinutes { get; set; } = 0;
      
        public DateTime Expiration { get; set; }
    }
}
