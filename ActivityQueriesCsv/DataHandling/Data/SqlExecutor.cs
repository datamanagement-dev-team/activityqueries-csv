using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace DataHandling.Data
{
    public class SqlExecutor
    {
        private readonly string _connectionString;

        public SqlExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> StoredProcedureExistsAsync(string procedureName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM sys.procedures WHERE Name = @Name",
                    new { Name = procedureName }
                );
                return result > 0;
            }
        }

        public async Task<List<dynamic>> ExecuteStoredProcedureInBatches(DbRequest request, int pageSize = 100)
        {
            var results = new List<dynamic>();
            int pageNumber = 1;
            bool hasMoreRows;

            do
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    IDictionary<string, object> paramDict = new ExpandoObject();

                    if (request.Parameters != null)
                    {
                        foreach (var prop in request.Parameters.GetType().GetProperties())
                        {
                            paramDict[prop.Name] = prop.GetValue(request.Parameters);
                        }
                    }

                    paramDict["PageNumber"] = pageNumber;
                    paramDict["PageSize"] = pageSize;

                    try
                    {
                        var batch = (await connection.QueryAsync<dynamic>(
                            request.StoredProcedureName,
                            (object)paramDict,
                            commandType: System.Data.CommandType.StoredProcedure)).ToList();

                        results.AddRange(batch);
                        hasMoreRows = batch.Count == pageSize;
                        pageNumber++;
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Message.Contains("expects parameter") || ex.Message.Contains("too many arguments"))
                        {
                            Console.WriteLine($"Wrong parameters! ---->  ***{ex.Message}***");
                        }
                        else
                        {
                            switch (ex.Number)
                            {
                                case 50001:
                                    // Business rule: date range too large
                                    Console.WriteLine($"Date Range Error:  {ex.Message}");
                                    break;
                                case 50002:
                                    // Parameter validation
                                    Console.WriteLine($"Error: Required params -> {ex.Message}");
                                    break;
                                case 50999:
                                    // Catch-all for unexpected errors from the SP
                                    Console.WriteLine($"Unexpected database error: {ex.Message}");
                                    break;
                                default:
                                    // Other SQL errors (connection, timeout, etc.)
                                    Console.WriteLine($"SQL Error ({ex.Number}): {ex.Message}");
                                    break;
                            }
                        }
                        throw;
                    }
                }
            } while (hasMoreRows);

            return results;
        }

    }
}
