using Dotnet_Developer_Test_Assignment.models;

namespace Dotnet_Developer_Test_Assignment.Repositories.Interfaces
{
    public interface ILogRepository
    {
        void Add(BlockedAttemptLog log);
        IEnumerable<BlockedAttemptLog> GetAll();
    }
}
