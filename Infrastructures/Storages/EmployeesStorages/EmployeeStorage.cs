using System.Data.SqlClient;
using System.Data;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.Dashboards;
using Microsoft.Extensions.Configuration;

namespace Infrastructures.Storages.EmployeesStorages
{
    public class EmployeStorage : IEmployeStorage
    {
        private readonly string _connectionString;

        public EmployeStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }
        string numberEquipe = @"SELECT COUNT(*) AS TotalEquipes
FROM [db_aa9d4f_gestionpersonnel].[dbo].[Equipes];";


        
        private const string _selectByIdQuery = @"
            SELECT E.EmployeID, E.Nom, E.Prenom, E.DateDeNaissance, E.NSecuriteSocial, E.Adresse, E.GroupSanguin, 
                   E.NTelephone, E.FonctionID, E.DateEntree, E.DateSortie, E.SitiationFamiliale, 
                   E.Photo, F.NomFonction
            FROM Employes E
            INNER JOIN Fonctions F ON E.FonctionID = F.FonctionID
            WHERE E.EmployeID = @id";
        private const string _insertQuery = "INSERT INTO Employes (Nom, Prenom, DateDeNaissance, NSecuriteSocial, Adresse, GroupSanguin, NTelephone, FonctionID, DateEntree, DateSortie, SitiationFamiliale, Photo) VALUES (@Nom, @Prenom, @DateDeNaissance, @NSecuriteSocial, @Adresse, @GroupSanguin, @NTelephone, @FonctionID, @DateEntree, @DateSortie, @SitiationFamiliale, @Photo); SELECT SCOPE_IDENTITY();";
        private const string _updateQuery = @"
            UPDATE Employes
            SET 
                Nom = @Nom,
                Prenom = @Prenom,
                DateDeNaissance = @DateDeNaissance,
                NSecuriteSocial = @NSecuriteSocial,
                Adresse = @Adresse,
                GroupSanguin = @GroupSanguin,
                NTelephone = @NTelephone,
                FonctionID = @FonctionID,
                Journee = @Journee,
                DateEntree = @DateEntree,
                DateSortie = @DateSortie,
                SituationFamiliale = @SituationFamiliale,
                Photo = @Photo
            WHERE EmployeID = @EmployeID;
        "; private const string _deleteQuery = "UPDATE Employes SET status = @Status, DateSortie = @DateSortie WHERE EmployeID = @EmployeID;";

        private const string _selectByFunctionIdQuery = @"
            SELECT E.EmployeID, E.Nom, E.Prenom, E.DateDeNaissance, E.NSecuriteSocial, E.Adresse, E.GroupSanguin, 
                   E.NTelephone, E.FonctionID, E.DateEntree, E.DateSortie, E.SitiationFamiliale, 
                   E.Photo, F.NomFonction
            FROM Employes E
            INNER JOIN Fonctions F ON E.FonctionID = F.FonctionID
            WHERE E.FonctionID = @FonctionID AND E.status = 1";

        private const string _selectEmployeeIdByNameAndFunctionQuery = @"
    SELECT E.EmployeID
    FROM Employes E
    INNER JOIN Fonctions F ON E.FonctionID = F.FonctionID
    WHERE E.Nom = @Nom AND E.Prenom = @Prenom AND F.NomFonction = @NomFonction";


        private const string countNumberOfemployesbyFunctionQuery = @"  SELECT 
    f.[NomFonction], 
    e.[FonctionID], 
    COUNT(*) AS NumberOfEmployees
FROM 
   [db_aa9d4f_gestionpersonnel].[dbo].[Employes] e 
INNER JOIN 
    [db_aa9d4f_gestionpersonnel].[dbo].[Fonctions] f
ON 
    e.[FonctionID] = f.[FonctionID]
 WHERE 
        e.status = 1
GROUP BY 
    e.[FonctionID],f.[NomFonction] 
    
ORDER BY 
    NumberOfEmployees DESC 
";

        private const string selectEmployesByStatusQuery = @"
         SELECT *
            FROM Employes E
            INNER JOIN Fonctions F ON E.FonctionID = F.FonctionID
            WHERE E.Status = @Status";


        private static Employe GetEmployeFromDataRow(DataRow row)
        {
            return new Employe
            {
                EmployeID = (int)row["EmployeID"],
                Nom = (string)row["Nom"],
                Prenom = (string)row["Prenom"],
                DateDeNaissance = (DateTime)row["DateDeNaissance"],
                NSecuriteSocial = (string)row["NSecuriteSocial"],
                Adresse = (string)row["Adresse"],
                GroupSanguin = (string)row["GroupSanguin"],
                NTelephone = (string)row["NTelephone"],
                FonctionID = (int)row["FonctionID"],
                DateEntree = (DateTime)row["DateEntree"],
                DateSortie = row["DateSortie"] != DBNull.Value ? (DateTime)row["DateSortie"] : null,
                SituationFamiliale = (string)row["SituationFamiliale"],
                Photo = row["Photo"] as byte[],
                FonctionName = row["NomFonction"].ToString(),
                Journee = row["Journee"] != DBNull.Value ? Convert.ToInt32(row["Journee"]) : 0,



            };
        }

        public async Task<int> SelectCountEquipes()
        {
            await using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using SqlCommand cmd = new SqlCommand(numberEquipe, conn);

            return (int)(await cmd.ExecuteScalarAsync() ?? 0);
        }
       

        public async Task<Employe?> GetById(int id)
        {
            await using var connection = new SqlConnection(_connectionString);

            SqlCommand cmd = new(_selectByIdQuery, connection);
            cmd.Parameters.AddWithValue("@id", id);

            DataTable dataTable = new();
            SqlDataAdapter da = new(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            return dataTable.Rows.Count == 0 ? null : GetEmployeFromDataRow(dataTable.Rows[0]);
        }

        public async Task Add(Employe employe)
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new(_insertQuery, connection);
            cmd.Parameters.AddWithValue("@Nom", employe.Nom);
            cmd.Parameters.AddWithValue("@Prenom", employe.Prenom);
            cmd.Parameters.AddWithValue("@DateDeNaissance", employe.DateDeNaissance);
            cmd.Parameters.AddWithValue("@NSecuriteSocial", employe.NSecuriteSocial);
            cmd.Parameters.AddWithValue("@Adresse", employe.Adresse);
            cmd.Parameters.AddWithValue("@GroupSanguin", employe.GroupSanguin);
            cmd.Parameters.AddWithValue("@NTelephone", employe.NTelephone);
            cmd.Parameters.AddWithValue("@FonctionID", employe.FonctionID);
            cmd.Parameters.AddWithValue("@DateEntree", employe.DateEntree);
            cmd.Parameters.AddWithValue("@DateSortie", employe.DateSortie ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@SitiationFamiliale", employe.SituationFamiliale);
            cmd.Parameters.AddWithValue("@Photo", employe.Photo ?? (object)DBNull.Value);

            await connection.OpenAsync();
            var id = await cmd.ExecuteScalarAsync();
            employe.EmployeID = Convert.ToInt32(id);
        }

        public async Task Update(Employe employe)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // ✅ Use a transaction for safety
            await using var transaction = await connection.BeginTransactionAsync();

            
            
             await using SqlCommand cmd = new(_updateQuery, connection, (SqlTransaction)transaction);

                // ✅ Parameter assignments
                cmd.Parameters.AddWithValue("@Nom", employe.Nom);
                cmd.Parameters.AddWithValue("@Prenom", employe.Prenom);
                cmd.Parameters.AddWithValue("@DateDeNaissance", employe.DateDeNaissance);
                cmd.Parameters.AddWithValue("@NSecuriteSocial", employe.NSecuriteSocial);
                cmd.Parameters.AddWithValue("@Adresse", employe.Adresse);
                cmd.Parameters.AddWithValue("@GroupSanguin", employe.GroupSanguin);
                cmd.Parameters.AddWithValue("@NTelephone", employe.NTelephone);
                cmd.Parameters.AddWithValue("@FonctionID", employe.FonctionID);
                cmd.Parameters.AddWithValue("@Journee", employe.Journee);
                cmd.Parameters.AddWithValue("@DateEntree", employe.DateEntree);
                cmd.Parameters.AddWithValue("@DateSortie", employe.DateSortie ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SituationFamiliale", employe.SituationFamiliale ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Photo", employe.Photo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@EmployeID", employe.EmployeID);
                
                // ✅ Execute update
                await cmd.ExecuteNonQueryAsync();

                // ✅ Commit transaction
                await transaction.CommitAsync();
            
        }

        public async Task UpdateStatusofEmploye(int id,EmployeeStatus status)
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new(_deleteQuery, connection);
            cmd.Parameters.AddWithValue("@EmployeID", id);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@DateSortie", DateTime.Now);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        

        public async Task<int> GetTotalNumberOfEmployees()
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new("CalculerNombreTotalEmployes", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            var param = new SqlParameter("@nombreTotalEmployes", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)cmd.Parameters["@nombreTotalEmployes"].Value;
        }

        public async Task<decimal> GetTotalSalaryForMonth(DateTime month)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                SqlCommand cmd = new("CalculerTotalSalairesDansUnMois", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@mois", month);

                var param = new SqlParameter("@totalSalaires", SqlDbType.Decimal);
                param.Direction = ParameterDirection.Output;
                param.Precision = 10;
                param.Scale = 2;
                cmd.Parameters.Add(param);

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                // Check if the output parameter value is DBNull or NULL
                if (cmd.Parameters["@totalSalaires"].Value == DBNull.Value || cmd.Parameters["@totalSalaires"].Value == null)
                {
                    return 0; // Or handle DBNull/NULL case as per your application logic
                }

                return Convert.ToDecimal(cmd.Parameters["@totalSalaires"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the total salary for the month.", ex);
            }
        }

        public async Task<List<Employe>> GetEmployeesByFunctionId(int fonctionId)
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new(_selectByFunctionIdQuery, connection);
            cmd.Parameters.AddWithValue("@FonctionID", fonctionId);

            DataTable dataTable = new();
            SqlDataAdapter da = new(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            return (from DataRow row in dataTable.Rows select GetEmployeFromDataRow(row)).ToList();
        }
        public async Task<int?> GetEmployeeIdByName(string nom, string prenom, string nomFonction)
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new(_selectEmployeeIdByNameAndFunctionQuery, connection);
            cmd.Parameters.AddWithValue("@Nom", nom);
            cmd.Parameters.AddWithValue("@Prenom", prenom);
            cmd.Parameters.AddWithValue("@NomFonction", nomFonction);

            await connection.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();

            // Check if result is DBNull
            if (result == DBNull.Value || result == null)
            {
                return null; // Employee not found
            }

            return Convert.ToInt32(result); // Return the EmployeeID
        }

        /*public async Task<List<Employee>> GetEmployeesBystatus(EmployeeStatus status)
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new(selectEmployesByStatusQuery, connection);

            DataTable dataTable = new();
            SqlDataAdapter da = new(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            return (from DataRow row in dataTable.Rows select GetEmployeFromDataRow(row)).ToList();
        }*/
      
         public async Task<List<Employe>> SelectEmployeesByStatus(EmployeeStatus status)
           {
               await using var connection = new SqlConnection(_connectionString);
               await connection.OpenAsync();
           
               await using var cmd = new SqlCommand(selectEmployesByStatusQuery, connection);

               cmd.Parameters.AddWithValue("@Status", (int)status);
           
               var dataTable = new DataTable();
               using var da = new SqlDataAdapter(cmd);
               da.Fill(dataTable);
               
               return (from DataRow row in dataTable.Rows
                       select GetEmployeFromDataRow(row)).ToList();
           }
         


        public async  Task<List<CountFunction>> SelectEmployeesCountByFunction()
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new(countNumberOfemployesbyFunctionQuery, connection);

            DataTable dataTable = new();
            SqlDataAdapter da = new(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            var result = new List<CountFunction>();

            foreach (DataRow row in dataTable.Rows)
            {
                var countFunction = new CountFunction
                {
                    Name = (string)row["NomFonction"],
                    FunctionId = (int)row["FonctionID"],
                    Total = (int)row["NumberOfEmployees"]
                };
                result.Add(countFunction);
            }

            return result;
        }
    }
}
