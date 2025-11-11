using GrhDz.Domains.Models.Primes;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Infrastructures.Storages.PrimesStorages
{
    public class PrimeStorage : IPrimeStorage
    {
        private readonly string _connectionString;
        public PrimeStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        private const string InsertQuery = @"INSERT INTO Primes 
    ([EmployeId], [Montant], [Date], [Description])
    VALUES (@EmployeId, @Montant, @Date, @Description);
    SELECT SCOPE_IDENTITY();";

        public async Task Add(PrimeType prime)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(InsertQuery, connection);

            cmd.Parameters.AddWithValue("@EmployeId", prime.EmployeID);
            cmd.Parameters.AddWithValue("@Montant", prime.Montant);
            cmd.Parameters.AddWithValue("@Date", prime.Date);
            cmd.Parameters.AddWithValue("@Description", prime.Description ?? "no comment");


            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync(); 
        }

    }
}
