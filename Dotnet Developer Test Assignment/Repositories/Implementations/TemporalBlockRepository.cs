using Dotnet_Developer_Test_Assignment.models;
using Dotnet_Developer_Test_Assignment.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace Dotnet_Developer_Test_Assignment.Repositories.Implementations
{
    public class TemporalBlockRepository : ITemporalBlockRepository
    {
        private readonly ConcurrentDictionary<string, TemporalBlock> _blocks = new();

        public bool Add(TemporalBlock block)
        {
            return _blocks.TryAdd(block.CountryCode.ToUpper(), block);
        }

        public void Remove(string countryCode)
        {
            _blocks.TryRemove(countryCode.ToUpper(), out _);
        }

        public IEnumerable<TemporalBlock> GetAll() => _blocks.Values;

        public bool Exists(string countryCode) => _blocks.ContainsKey(countryCode.ToUpper());
    }
}
