using GrhDz.Domains.Models.Avances;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructures.Storages.AvancesStorages
{
    public class AvanceStorage(IConfiguration configuration) : IAvanceStorage
    {
        private readonly string _connectionString = configuration.GetConnectionString("DBConnection") ?? throw new InvalidCastException("Connection string is missing or empty.");

        private const string SelectAllQuery = "SELECT * FROM Avances";
        private const string SelectByIdQuery = "SELECT * FROM Avances WHERE AvanceID = @id";
        private const string InsertQuery = "INSERT INTO Avances (EmployeID, Montant, Date, Description) " +
                                           "VALUES (@EmployeID, @Montant, @Date, @Description); SELECT SCOPE_IDENTITY();";
        private const string UpdateQuery = "UPDATE Avances SET EmployeID = @EmployeID, Montant = @Montant, " +
                                           "Date = @Date WHERE AvanceID = @AvanceID;";
        private const string DeleteQuery = "DELETE FROM Avances WHERE AvanceID = @AvanceID;";
        private const string SelectByDate = "SELECT * FROM Avances WHERE Date=@Date";
        private const string SelectTotaleAvances = "SELECT SUM(Montant)  FROM Avances WHERE YEAR(Date) = YEAR(@Date) AND MONTH(Date) = MONTH(@Date);";
        private const string SelectAvacebyDate = @"
       SELECT 
    a.AvanceID, 
    a.EmployeID, 
    e.Nom, 
    e.Prenom, 
    a.Montant, 
    a.Date,
    a.Description
FROM Avances a
INNER JOIN Employes e 
    ON a.EmployeID = e.EmployeID
WHERE a.Date >= DATEFROMPARTS(YEAR(@SelectedDate), MONTH(@SelectedDate), 1)
  AND a.Date < DATEADD(MONTH, 1, DATEFROMPARTS(YEAR(@SelectedDate), MONTH(@SelectedDate), 1))
ORDER BY a.Date DESC;"; 


        private static AvanceModel GetAvanceFromDataRow(DataRow row)
        {
            return new AvanceModel
            {
                AvanceID = (int)row["AvanceID"],
                EmployeID = (int)row["EmployeID"],
                Montant = (decimal)row["Montant"],
                Date = (DateTime)row["Date"]
            };
        }
        public async Task<List<AvanceModel>> GetByEmployeIdInMonth(int employeId, DateTime selectedMonth)
        {
            var avances = new List<AvanceModel>();

            // Set start of the month: e.g., 2025-07-01
            var startOfMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1);

            // Set end of the month: e.g., 2025-07-31
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
        SELECT * FROM Avances 
        WHERE EmployeID = @EmployeID 
        AND Date >= @StartOfMonth 
        AND Date <= @EndOfMonth", connection);

            cmd.Parameters.AddWithValue("@EmployeID", employeId);
            cmd.Parameters.AddWithValue("@StartOfMonth", startOfMonth);
            cmd.Parameters.AddWithValue("@EndOfMonth", endOfMonth);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                avances.Add(GetAvanceFromDataRow(row));
            }

            return avances;
        }



        public async Task<List<AvanceModel>> GetAll()
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(SelectAllQuery, connection);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            return (from DataRow row in dataTable.Rows select GetAvanceFromDataRow(row)).ToList();
        }

        public async Task<AvanceModel?> GetById(int avanceId)
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(SelectByIdQuery, connection);
            cmd.Parameters.AddWithValue("@id", avanceId);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            return (dataTable.Rows.Count == 0) ? null : GetAvanceFromDataRow(dataTable.Rows[0]);
        }

        public async Task<int> Add(AvanceModel avanceModel)
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(InsertQuery, connection);

            cmd.Parameters.AddWithValue("@EmployeID", avanceModel.EmployeID);
            cmd.Parameters.AddWithValue("@Montant", avanceModel.Montant);
            cmd.Parameters.AddWithValue("@Date", avanceModel.Date);
            cmd.Parameters.Add(new SqlParameter("@Description", avanceModel.Description ?? "No Comment"));

            await connection.OpenAsync();
            var id = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(id);
        }

        public async Task Update(AvanceModel avanceModel)
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(UpdateQuery, connection);

            cmd.Parameters.AddWithValue("@EmployeID", avanceModel.EmployeID);
            cmd.Parameters.AddWithValue("@Montant", avanceModel.Montant);
            cmd.Parameters.AddWithValue("@Date", avanceModel.Date);
            cmd.Parameters.AddWithValue("@AvanceID", avanceModel.AvanceID);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(int avanceId)
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(DeleteQuery, connection);
            cmd.Parameters.AddWithValue("@AvanceID", avanceId);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<AvanceModel>> GetByDate(DateTime date)
        {
            var avances = new List<AvanceModel>();

            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(SelectByDate, connection);
            cmd.Parameters.AddWithValue("@Date", date);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                avances.Add(GetAvanceFromDataRow(row));
            }

            return avances;
        }
        public async Task<decimal> GetTotale(DateTime date)
        {
            decimal totaleAvances = 0m;

            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand( SelectTotaleAvances,connection);

            cmd.Parameters.AddWithValue("@Date", date);

            await connection.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();

            if (result != null && result != DBNull.Value)
            {
                totaleAvances = Convert.ToDecimal(result);
            }

            return totaleAvances;
        }
        public async Task<List<AvanceModel>> GetAvancesWithEmployee(DateTime specificDate)
        {
            var avances = new List<AvanceModel>();

            await using var connection = new SqlConnection(_connectionString);


           await using var cmd = new SqlCommand(SelectAvacebyDate, connection);

            // Add the specific date parameter to the query
            cmd.Parameters.AddWithValue("@SelectedDate", specificDate);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                var avance = new AvanceModel
                {
                    AvanceID = (int)row["AvanceID"],
                    EmployeID = (int)row["EmployeID"],
                    NomEmployee = row["Nom"].ToString(),
                    PrenomEmployee = row["Prenom"].ToString(),
                    Montant = (decimal)row["Montant"],
                    Date = (DateTime)row["Date"]
                };

                avances.Add(avance);
            }

            return avances;
        }


    }
}