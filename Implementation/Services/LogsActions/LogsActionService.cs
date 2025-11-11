using GrhDz.Domains.Models.Logs;
using Implementation.Services.LogsAction;
using Infrastructures.Storages.LogActionStorage;

namespace Implementation.Services.LogsActions
{
    public class LogsActionService(LogActionStorage logsActionStorage) : ILogsActionService
    {
      

    

        public async Task settLog(LogAction logActions)
        {
            try
            {
                await logsActionStorage.InsertLog(logActions);
            }
            catch (Exception exception)
            {
                Console.WriteLine("error", exception);
                throw;
            }
        }

        public async Task<List<LogAction>> GetAllLogs()
        {
            try
            {
                return await logsActionStorage.SelectAllLogs();
            }
            catch (Exception exception)
            {
                Console.WriteLine("error", exception);
                throw;
            }
        }
    }
}
