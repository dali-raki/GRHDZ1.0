using GrhDz.Domains.Models.Primes;
using GrhDz.Domains.Models.Remboursements;
using Microsoft.Extensions.Configuration;
using System.Data;
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



        private const string getprimequery = @"  
                           SELECT Id, EmployeID, Montant, Date, Description FROM Primes 
        WHERE EmployeID = @EmployeID 
        AND Date >= @StartOfMonth 
        AND Date <= @EndOfMonth";
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
        public async Task<List<PrimeType>> SelectByEmployeIdInMonth(int employeId, DateTime selectedMonth)
        {
            var primes = new List<PrimeType>();

            // Get first and last day of the selected month
            var startOfMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            await using var connection = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(getprimequery, connection);

            cmd.Parameters.AddWithValue("@EmployeID", employeId);
            cmd.Parameters.AddWithValue("@StartOfMonth", startOfMonth);
            cmd.Parameters.AddWithValue("@EndOfMonth", endOfMonth);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            da.Fill(dataTable); // SqlDataAdapter.Fill is synchronous

            // Map DataTable rows to PrimeType objects
            foreach (DataRow row in dataTable.Rows)
            {
                var prime = new PrimeType
                {
                    EmployeID = Convert.ToInt32(row["EmployeId"]),
                    Montant = Convert.ToDecimal(row["Montant"]),
                    Date = Convert.ToDateTime(row["Date"]),
                    Description = row["Description"] == DBNull.Value ? "no comment" : row["Description"].ToString()
                };

                primes.Add(prime);
            }

            return primes;
        }

    }
}
