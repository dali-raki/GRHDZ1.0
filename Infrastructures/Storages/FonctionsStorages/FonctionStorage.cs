using System.Data.SqlClient;
using GrhDz.Domains.Models.Fonctions;
using Microsoft.Extensions.Configuration;

namespace Infrastructures.Storages.FonctionsStorages
{
    public class FonctionStorage : IFonctionStorage
    {
        private readonly string _connectionString;

        private const string selectfunctionbyid = "SELECT FonctionID, NomFonction FROM Fonctions WHERE FonctionID = @FonctionID"; // Ensure this matches your table name
        private const string insertfunction = "INSERT INTO Fonctions (NomFonction) VALUES (@NomFonction)";
        private const string updatefunction = "UPDATE Fonctions SET NomFonction = @NomFonction WHERE FonctionID = @FonctionID";
        private const string deleteQuery = "DELETE FROM Fonctions WHERE FonctionID = @FonctionID";
        private const string checkQuery = "SELECT COUNT(*) FROM Employes WHERE FonctionID = @FonctionID";
        public FonctionStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        public async Task<List<Fonction>> GetAll()
        {
            var fonctions = new List<Fonction>();
            string query = "SELECT FonctionID, NomFonction FROM Fonctions";

            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        fonctions.Add(new Fonction
                        {
                            FonctionID = reader.GetInt32(0),
                            NomFonction = reader.GetString(1)
                        });
                    }
                }
            }
            return fonctions;
        }

        public async Task Add(Fonction fonction)
        {


            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(insertfunction, connection);
                command.Parameters.AddWithValue("@NomFonction", fonction.NomFonction);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task Update(Fonction fonction)
        {


            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(updatefunction, connection);
                command.Parameters.AddWithValue("@NomFonction", fonction.NomFonction);
                command.Parameters.AddWithValue("@FonctionID", fonction.FonctionID);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task Delete(int fonctionId)
        {
            try
            {
                // Check if the Fonction is referenced by any employee
                await using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@FonctionID", fonctionId);
                    int count = (int)await checkCommand.ExecuteScalarAsync();

                    if (count > 0)
                    {
                        throw new InvalidOperationException("The function cannot be deleted because it is referenced by employees.");
                    }

                  
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@FonctionID", fonctionId);
                    await deleteCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while deleting the function: {ex.Message}");
            }
        }



        public async Task<Fonction> GetById(int fonctionId)
        {
            Fonction fonction = null;

            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(selectfunctionbyid, connection);
                command.Parameters.AddWithValue("@FonctionID", fonctionId);
                await connection.OpenAsync();
                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        fonction = new Fonction
                        {
                            FonctionID = reader.GetInt32(0),
                            NomFonction = reader.GetString(1)
                        };
                    }
                }
            }
            return fonction;
        }
    }
}
