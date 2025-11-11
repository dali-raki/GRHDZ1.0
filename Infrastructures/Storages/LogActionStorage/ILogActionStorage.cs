using GrhDz.Domains.Models.Logs;

namespace Infrastructures.Storages.LogActionStorage
{
    public interface ILogActionStorage
    {
        Task<List<LogAction>> SelectAllLogs();
    }
}