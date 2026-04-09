using Dotnet_Developer_Test_Assignment.models;
using Dotnet_Developer_Test_Assignment.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace Dotnet_Developer_Test_Assignment.Repositories.Implementations
{
    public class LogRepository : ILogRepository
    {
        private readonly ConcurrentBag<BlockedAttemptLog> _logs = new();

        public void Add(BlockedAttemptLog log)
        {
            _logs.Add(log);
        }

        public IEnumerable<BlockedAttemptLog> GetAll()
        {
            return _logs;
        }
    }
}
