using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Remboursements;
using Infrastructures.Storages.RemboursementsStorages;

namespace Implementation.Services.Remboursement
{
    public class RemboursementService(RemboursementStorage remboursementStorage) : IRemboursementService
    {


        public async Task<Result<bool>> AddAsync(RemboursementType remboursement)
        {
            try
            {
                if (remboursement == null)
                    return Result.Failure<bool>(Error.Validation("Remboursement cannot be null."));

                await remboursementStorage.Add(remboursement);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<RemboursementType>>> GetByEmployeIdInMonthAsync(int employeId, DateTime selectedMonth)
        {
            try
            {
                var remboursements = await remboursementStorage.GetByEmployeIdInMonth(employeId, selectedMonth);
                return Result.Success(remboursements);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<RemboursementType>>(Error.Exception(ex));
            }
        }



    }
}
