
using Dotnet_Developer_Test_Assignment.Repositories.Implementations;
using Dotnet_Developer_Test_Assignment.Repositories.Interfaces;

namespace Dotnet_Developer_Test_Assignment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSingleton<ICountryRepository, CountryRepository>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IIPService, IPService>();
            builder.Services.AddSingleton<ILogRepository, LogRepository>();
            builder.Services.AddSingleton<ITemporalBlockRepository, TemporalBlockRepository>();
            builder.Services.AddHostedService<TemporalBlockCleanupService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
