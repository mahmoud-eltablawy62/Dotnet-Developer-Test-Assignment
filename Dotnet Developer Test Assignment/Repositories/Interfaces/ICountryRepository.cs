using Dotnet_Developer_Test_Assignment.models;

namespace Dotnet_Developer_Test_Assignment.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        bool Add(Country country);

        bool Remove(string code);

        bool Exists(string code);

        IEnumerable<Country> GetAll();
    }
}
