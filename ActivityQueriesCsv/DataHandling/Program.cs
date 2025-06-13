using System;
using System.Net;
using System.Threading.Tasks;
using DataHandling.Data;
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

        var request = new DbRequest
        {
            StoredProcedureName = "GetActivityLogs",
            Parameters = new
            {
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 3, 19)
            }
        };

        if(connectionString is not null)
        {
            var executor = new SqlExecutor(connectionString);

            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex}");
            }
        }
        
    }
}
