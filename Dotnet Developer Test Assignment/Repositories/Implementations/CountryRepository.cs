using Dotnet_Developer_Test_Assignment.models;
using Dotnet_Developer_Test_Assignment.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace Dotnet_Developer_Test_Assignment.Repositories.Implementations
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ConcurrentDictionary<string, Country> _countries = new();

        public bool Add(Country country)
        {
            return _countries.TryAdd(country.Code.ToUpper(), country);
        }

        public bool Remove(string code)
        {
            return _countries.TryRemove(code.ToUpper(), out _);
        }

        public bool Exists(string code)
        {
            return _countries.ContainsKey(code.ToUpper());
        }

        public IEnumerable<Country> GetAll()
        {
            return _countries.Values;
        }
    }
}
