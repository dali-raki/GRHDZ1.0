using GrhDz.Domains.Models.EquipePost;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructures.Storages.PostesStorages
{
    public class PosteStorage: IPosteStorage
    {
        private readonly string _connectionString;

        public PosteStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        private const string insertPosteCompleteQuery = @"
            INSERT INTO [db_aa9d4f_gestionpersonnel].[dbo].[PosteComplete] ([IdPoste], [IdEquipe], [Date])
            VALUES (@IdPoste, @IdEquipe, @Date);
            SELECT SCOPE_IDENTITY();";
        private const string insertEmployePosteQuery = @"
            INSERT INTO [db_aa9d4f_gestionpersonnel].[dbo].[EmployePoste] ([IdEmploye], [Date],[EquipeID])
            VALUES (@IdEmploye, @Date,@EquipeID);";

        private const string updateOrInsertTotalePostesQuery = @"
            MERGE [db_aa9d4f_gestionpersonnel].[dbo].[TotalePostes] AS target
            USING (
                SELECT [IdEmploye], COUNT(*) AS TotalePostes
                FROM [db_aa9d4f_gestionpersonnel].[dbo].[EmployePoste]
                WHERE YEAR([Date]) = @Year AND MONTH([Date]) = @Month
                GROUP BY [IdEmploye]
            ) AS source
            ON target.IdEmploye = source.IdEmploye
               AND MONTH(target.[Date]) = @Month
               AND YEAR(target.[Date]) = @Year
            WHEN MATCHED THEN
                UPDATE SET target.TotalePostes = source.TotalePostes
            WHEN NOT MATCHED THEN
                INSERT (IdEmploye, [Date], TotalePostes)
                VALUES (source.IdEmploye, DATEFROMPARTS(@Year, @Month,1), source.TotalePostes);";
        public async Task InsererDonneesPoste(string idPoste, int idEquipe, DateTime date, List<int> idEmployes)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                
            

                int idPosteComplete;
                await using (SqlCommand command = new SqlCommand(insertPosteCompleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@IdPoste", idPoste);
                    command.Parameters.AddWithValue("@IdEquipe", idEquipe);
                    command.Parameters.AddWithValue("@Date", date);

                    idPosteComplete = Convert.ToInt32(await command.ExecuteScalarAsync());
                }



             

                foreach (int idEmploye in idEmployes)
                {
                    await using (SqlCommand command = new SqlCommand(insertEmployePosteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdEmploye", idEmploye);
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@EquipeID", idEquipe);
                        await command.ExecuteNonQueryAsync();
                    }
                }



              

                using (SqlCommand command = new SqlCommand(updateOrInsertTotalePostesQuery, connection))
                {
                    command.Parameters.AddWithValue("@Year", date.Year);
                    command.Parameters.AddWithValue("@Month", date.Month);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
          public async Task<(List<EmployePosts> EmployePosts, EquipeSalaires EquipeSalaires)> SelectEquipeSalairesAndPostes(int equipeId, DateTime date)
        {
            var employePosts = new List<EmployePosts>();
            EquipeSalaires equipeSalaires = null;

            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                await using (SqlCommand command = new SqlCommand("GetEquipeSalairesAndPostes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EquipeID", equipeId);
                    command.Parameters.AddWithValue("@Date", date);

                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                 
                        while (await reader.ReadAsync())
                        {
                            employePosts.Add(new EmployePosts
                            {
                                EmployeNomPrenom = reader["EmployeNomPrenom"].ToString(),
                                TotalPostsEmploye = Convert.ToInt32(reader["TotalPostsEmploye"])
                            });
                        }

          
                        if (await reader.NextResultAsync() && await reader.ReadAsync())
                        {
                            equipeSalaires = new EquipeSalaires
                            {
                                NomEquipe = reader["NomEquipe"].ToString(),
                                TotalePostes = Convert.ToInt32(reader["TotalePostes"]),
                                SalaireTotale = Convert.ToDecimal(reader["SalaireTotale"])
                            };
                        }
                    }
                }
            }

            return (employePosts, equipeSalaires);
        }
    }
}