using GrhDz.Domains.Models.Dashboards;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Infrastructures.Storages.DashboardStorages
{
    public class DashboardStorage(IConfiguration configuration) : IDashboardStorage
    {
        private readonly string _connectionString = configuration.GetConnectionString("DBConnection") ?? throw new InvalidOperationException($"Connection string is missing or empty.");
            

        
        //private const string getTotalDetteAndAvanceQuery = "dbo.GetDashbordData";

        string differenceofabsence = @"
    DECLARE @Today DATE = GETDATE();

-- Current month and year
DECLARE @Month1 INT = MONTH(@Today);
DECLARE @Year1 INT = YEAR(@Today);

-- Last month and year
DECLARE @Month2 INT = MONTH(DATEADD(MONTH, -1, @Today));
DECLARE @Year2 INT = YEAR(DATEADD(MONTH, -1, @Today));

WITH Absences AS (
    SELECT 
        YEAR([Date]) AS Year,
        MONTH([Date]) AS Month,
        COUNT(*) AS NombreAbsences
    FROM [db_aa9d4f_gestionpersonnel].[dbo].[Pointage]
    WHERE [HeureEntree] IS NULL OR [HeureSortie] IS NULL
    GROUP BY YEAR([Date]), MONTH([Date])
)
SELECT 
    ISNULL(A1.NombreAbsences, 0) AS Absences_Mois1,
    ISNULL(A2.NombreAbsences, 0) AS Absences_Mois2,
    ISNULL(A1.NombreAbsences, 0) - ISNULL(A2.NombreAbsences, 0) AS Difference
FROM 
    (SELECT NombreAbsences FROM Absences WHERE Month = @Month1 AND Year = @Year1) A1
FULL JOIN 
    (SELECT NombreAbsences FROM Absences WHERE Month = @Month2 AND Year = @Year2) A2
    ON 1 = 1;
";

        string differenceofpresence = @"
    DECLARE @Today DATE = GETDATE();

-- Current month and year
DECLARE @Month1 INT = MONTH(@Today);
DECLARE @Year1 INT = YEAR(@Today);

-- Last month and year
DECLARE @Month2 INT = MONTH(DATEADD(MONTH, -1, @Today));
DECLARE @Year2 INT = YEAR(DATEADD(MONTH, -1, @Today));

-- CTE for absences
WITH Absences AS (
    SELECT 
        YEAR([Date]) AS Year,
        MONTH([Date]) AS Month,
        COUNT(*) AS NombreAbsences
    FROM [db_aa9d4f_gestionpersonnel].[dbo].[Pointage]
    WHERE [HeureEntree] IS NULL OR [HeureSortie] IS NULL
    GROUP BY YEAR([Date]), MONTH([Date])
),

-- CTE for presences
Presences AS (
    SELECT 
        YEAR([Date]) AS Year,
        MONTH([Date]) AS Month,
        COUNT(*) AS NombrePresences
    FROM [db_aa9d4f_gestionpersonnel].[dbo].[Pointage]
    WHERE [HeureEntree] IS NOT NULL AND [HeureSortie] IS NOT NULL
    GROUP BY YEAR([Date]), MONTH([Date])
)

-- Final SELECT with differences
SELECT 
    ISNULL(P1.NombrePresences, 0) AS Presences_Mois1,
    ISNULL(P2.NombrePresences, 0) AS Presences_Mois2,
    ISNULL(P1.NombrePresences, 0) - ISNULL(P2.NombrePresences, 0) AS Difference
FROM 
    (SELECT NombrePresences FROM Presences WHERE Month = @Month1 AND Year = @Year1) P1
FULL JOIN 
    (SELECT NombrePresences FROM Presences WHERE Month = @Month2 AND Year = @Year2) P2
    ON 1 = 1;";


        string numberEquipe = @"SELECT COUNT(*) AS TotalEquipes
FROM Equipes";



        private const string _getDetteByYearQuery = @"
            ;WITH Months AS (
                SELECT 1 AS MonthNumber UNION ALL
                SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL
                SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL
                SELECT 8 UNION ALL SELECT 9 UNION ALL SELECT 10 UNION ALL
                SELECT 11 UNION ALL SELECT 12
            )
            SELECT 
                m.MonthNumber,
                DATENAME(MONTH, DATEFROMPARTS(@SelectedYear, m.MonthNumber, 1)) AS MonthName,
                ISNULL(SUM(d.Montant), 0) AS TotalDette
            FROM Months m
            LEFT JOIN [dbo].[Dettes] d
                ON MONTH(d.[Date]) = m.MonthNumber
                AND YEAR(d.[Date]) = @SelectedYear
            GROUP BY m.MonthNumber
            ORDER BY m.MonthNumber;
        ";


        public async Task<List<DashboardModel>> GetDashboardDataAsync()
        {
            List<DashboardModel> dashboards = new List<DashboardModel>();

            await using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using SqlCommand cmd = new SqlCommand(_getDetteByYearQuery, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {

                dashboards.Add(getDashboardModelFromReader(reader));
            }

            return dashboards;
        }

        private static DashboardModel getDashboardModelFromReader(SqlDataReader reader)
        {
            return new DashboardModel
            {
                Month = reader.GetInt32(reader.GetOrdinal("Month")),
                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                Avance = reader.GetDecimal(reader.GetOrdinal("TotalAvance")),
                Dette = reader.GetDecimal(reader.GetOrdinal("TotalDette"))
            };
        }

        public async Task<List<DashboardPointage>> SelectPointageOfDashboard(int year, int month)
        {
            var result = new List<DashboardPointage>();

          await  using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                await using (SqlCommand cmd = new SqlCommand("PointageOfDashboard", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Month", month);

                    await using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var pointage = new DashboardPointage
                            {
                                EmployeID = reader.GetInt32(reader.GetOrdinal("EmployeID")),
                                NomComplet = reader.GetString(reader.GetOrdinal("NomComplet")),
                                NombrePresences = reader.GetInt32(reader.GetOrdinal("NombrePresences")),
                                NombreAbsences = reader.GetInt32(reader.GetOrdinal("NombreAbsences")),
                             NombreHeuresSupp = (decimal)reader["NumberHeuresSup8"],
                                EntryHeure = reader.IsDBNull(reader.GetOrdinal("FirstHeureEntree")) ? (TimeSpan?)null : reader.GetTimeSpan(reader.GetOrdinal("FirstHeureEntree")),
                                ExitHeure = reader.IsDBNull(reader.GetOrdinal("FirstHeureSortie")) ? (TimeSpan?)null : reader.GetTimeSpan(reader.GetOrdinal("FirstHeureSortie"))

                            };
                            result.Add(pointage);
                        }
                    }
                }
            }

            return result;
        }


        public async Task<DifferenceofPointage> SelectAbsenceComparison()
        {
            await using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new SqlCommand(differenceofabsence, conn);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new DifferenceofPointage
                {
                    PointageThisMonth = reader.GetInt32(reader.GetOrdinal("Absences_Mois1")),
                    PointageLastMonth = reader.GetInt32(reader.GetOrdinal("Absences_Mois2")),
                    Difference = reader.GetInt32(reader.GetOrdinal("Difference"))
                };
            }

            return null;
        }


        public async Task<DifferenceofPointage> SelectPresenceComparison()
        {
            await using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new SqlCommand(differenceofpresence, conn);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new DifferenceofPointage
                {
                    PointageThisMonth = reader.GetInt32(reader.GetOrdinal("Presences_Mois1")),
                    PointageLastMonth = reader.GetInt32(reader.GetOrdinal("Presences_Mois2")),
                    Difference = reader.GetInt32(reader.GetOrdinal("Difference"))
                };
            }

            return null;
        }

        public async Task<int> SelectCountEquipes()
        {
            await using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using SqlCommand cmd = new SqlCommand(numberEquipe, conn);
            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }

}
