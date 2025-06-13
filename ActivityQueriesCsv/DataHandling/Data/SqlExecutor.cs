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
                        if (ex.Number == 50001)
                            throw new InvalidOperationException("Date range cannot exceed 3 months.", ex);
                        throw;
                    }
                }
            } while (hasMoreRows);

            return results;
        }

    }
}
