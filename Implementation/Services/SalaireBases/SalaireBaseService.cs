using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.SalairesBase;
using Implementation.Services.SalaireBase;
using Infrastructures.Storages.SalairesBaseStorages;
using Microsoft.Identity.Client.Extensions.Msal;

namespace Implementation.Services.SalaireBases
{
    public class SalaireBaseService(SalaireBaseStorage storage) : ISalaireBaseService
    {

        public async Task<Result<List<SalairesBase>>> GetAll()
        {
            try
            {
                var items = await storage.GetAll();
                return items;
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result<SalairesBase>> GetById(int idSalaireBase)
        {
            try
            {
                var item = await storage.GetById(idSalaireBase);
                return item;

            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result<List<SalairesBase>>> GetByEmployeeId(int employeeId)
        {
            try
            {
                var items = await storage.GetByEmployeeId(employeeId);
                return items;
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result<int>> Add(SalairesBase salairesBase)
        {
            try
            {
                var id = await storage.Add(salairesBase);
                return Result<int>.Success(id);
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result> Update(SalairesBase salairesBase)
        {
            try
            {
                await storage.Update(salairesBase);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result> Delete(int idSalaireBase)
        {
            try
            {   
                await storage.Delete(idSalaireBase);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }
    }
}
