

using GrhDz.Domains.Models.Logs;

namespace Implementation.Services.LogsAction
{
    public interface ILogsActionService
    {
      public  Task settLog(LogAction logActions);

        public Task<List<LogAction>> GetAllLogs();
    }
}
