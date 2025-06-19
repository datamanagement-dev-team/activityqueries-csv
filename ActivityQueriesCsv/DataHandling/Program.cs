using System;
using System.Net;
using System.Threading.Tasks;
using DataHandling.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        // Build configuration
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string connectionString = config.GetConnectionString("ActivityQueriesCsvConnection")!;

        //Request wrong parameters
        //var request = new DbRequest
        //{
        //    StoredProcedureName = "GetActivityLogs",
        //    Parameters = new
        //    {
        //        StartDate = new DateTime(2025, 1, 1),
        //        EndDate = new DateTime(2025, 3, 19),
        //        OK = ""
        //    }
        //};

        // Request correct parameters but big date range
        var request = new DbRequest
        {
            StoredProcedureName = "GetActivityLogs",
            Parameters = new
            {
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 12, 19)
            }
        };

        // Request correct parameters wrong stored procedure name
        //var request = new DbRequest
        //{
        //    StoredProcedureName = "GetActivityLogss",
        //    Parameters = new
        //    {
        //        StartDate = new DateTime(2025, 1, 1),
        //        EndDate = new DateTime(2025, 3, 19)
        //    }
        //};

        // Request correct parameters 
        //var request = new DbRequest
        //{
        //    StoredProcedureName = "GetActivityLogs",
        //    Parameters = new
        //    {
        //        StartDate = new DateTime(2025, 1, 1),
        //        EndDate = new DateTime(2025, 3, 19)
        //    }
        //};

        if (connectionString is not null)
        {
            var executor = new SqlExecutor(connectionString);

            try
            {
                bool spExists = await executor.StoredProcedureExistsAsync(request.StoredProcedureName);
                if (!spExists)
                {
                    Console.WriteLine($"Stored procedure '{request.StoredProcedureName}' does not exist.");
                    return;
                }

                var logs = await executor.ExecuteStoredProcedureInBatches(request, pageSize: 2);

                Console.WriteLine("Results:");
                foreach (var log in logs)
                {
                    foreach (var kv in (IDictionary<string, object>)log)
                    {
                        Console.Write($"{kv.Key}: {kv.Value}\t");
                    }
                    Console.WriteLine();
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"SP error: {ex.Message}");
            }
            catch (SqlException ex){
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex}");
            }
        }
        
    }
}
