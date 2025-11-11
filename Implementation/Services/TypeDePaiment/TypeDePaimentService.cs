using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.TypeDePaiment;
using Infrastructures.Storages.TypeDePaimentStorages;

namespace Implementation.Services.TypeDePaiment
{
    public class TypeDePaiementService(TypeDePaiementStorage typeDePaiementStorage) : ITypeDePaiementService
    {

        public async Task<Result<List<TypeDePaiement>>> GetAllAsync()
        {
            try
            {
                var items = await typeDePaiementStorage.GetAll();
                return Result<List<TypeDePaiement>>.Success(items);
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result<TypeDePaiement>> GetByIdAsync(int id)
        {
            try
            {
                var item = await typeDePaiementStorage.GetById(id);
                return Result<TypeDePaiement>.Success(item);
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result<int>> AddAsync(TypeDePaiement typeDePaiement)
        {
            try
            {
                await typeDePaiementStorage.Add(typeDePaiement); 
                return Result<int>.Success(typeDePaiement.TypePaiementID); 

            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result> UpdateAsync(TypeDePaiement typeDePaiement)
        {
            try
            {
                await typeDePaiementStorage.Update(typeDePaiement);
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
                await typeDePaiementStorage.Delete(id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }
    }
}
