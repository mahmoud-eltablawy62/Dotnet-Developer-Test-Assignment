using Dotnet_Developer_Test_Assignment.models;

namespace Dotnet_Developer_Test_Assignment.Repositories.Interfaces
{
    public interface ITemporalBlockRepository
    {
        bool Add(TemporalBlock block);
        void Remove(string countryCode);
        IEnumerable<TemporalBlock> GetAll();
        bool Exists(string countryCode);
    }
}
