using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Salaire;
using GrhDz.Domains.Models.Salaires;
using Implementation.Services.Salaire;
using Infrastructures.Storages.SalairesStorages;

namespace Implementation.Services.Salaires
{
    public class SalaireService(SalaireStorage salaireStorage) : ISalaireService
    {

        public async Task<Result<List<SalaireModel>>> GetAllAsync()
        {
            try
            {
                var items = await salaireStorage.GetAll();
                return Result<List<SalaireModel>>.Success(items);
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result<List<SalaireDetail>>> GetSalaireDetails()
        {
            try
            {
                var items =  salaireStorage.GetSalaireDetails();
                return items;
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result<SalaireModel?>> GetByIdAsync(int id)
        {
            try
            {
                var item = await salaireStorage.GetById(id);
                return Result<SalaireModel?>.Success(item);
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result> AddAsync(SalaireModel salaire)
        {
            try
            {
                await salaireStorage.Add(salaire);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result> UpdateAsync(SalaireModel salaire)
        {
            try
            {
                await salaireStorage.Update(salaire);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                await salaireStorage.Delete(id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result<List<SalaireDetail>>> GetSalariesByMonthAsync(DateTime mois)
        {
            try
            {
                var items = await salaireStorage.SelectSalariesByMonth(mois);
                return Result<List<SalaireDetail>>.Success(items);
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result> UpdateDetteAsync(int employeeId, decimal dette, DateTime mois)
        {
            try
            {
                await salaireStorage.UpdateDette(employeeId, dette, mois);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result<int>> SetMonthlySalariesAsync()
        {
            try
            {
                var id = await salaireStorage.InsertMonthlySalaries();
                return Result<int>.Success(id);
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }
    }
}

