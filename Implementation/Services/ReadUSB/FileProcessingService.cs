using GrhDz.Domains.Models.CheckInOut;
using Infrastructures.Storages.RecordStorages;
using Infrastructures.Storages.TransferData;

namespace Implementation.Services.ReadUSB
{
    public class FileProcessingService(ICheckInOutStorage checkInOutStorage, ITransferDataStorage transferDataStorage) : IFileProcessingService
    {
  
        private List<CheckInOutRecord> ParseFileContent(string fileContent)
        {
            var records = new List<CheckInOutRecord>();

            var lines = fileContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var fields = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (fields.Length >= 6)
                {
                    var record = new CheckInOutRecord
                    {
                        UserId = int.Parse(fields[0]),
                        CheckTime = DateTime.Parse(fields[1]),
                        CheckType = fields[2],
                        VerifyCode = int.Parse(fields[3]),
                        SensorId = int.Parse(fields[4]),
                        MemoInfo = fields.Length > 5 ? fields[5] : string.Empty,
                        WorkCode = fields.Length > 6 ? fields[6] : string.Empty,
                        Sn = fields.Length > 7 ? fields[7] : string.Empty,
                        UserExtFmt = fields.Length > 8 ? int.Parse(fields[8]) : 0
                    };

                    records.Add(record);
                }
            }

            return records;
        }

        public async Task ProcessFile(string fileContent)
        {
            var records = ParseFileContent(fileContent);
            await checkInOutStorage.InsertRecords(records);
            await transferDataStorage.TransfererEmployees();
            await transferDataStorage.TransfererPointages();
            await transferDataStorage.CalculeCofficient();
            await transferDataStorage.InsertOrUpdateRapportsPointage();
            await transferDataStorage.InsertOrUpdateSalaires();
        }
    }
}