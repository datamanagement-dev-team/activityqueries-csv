using BenchmarkDotNet.Running;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StoredProcedureToCsv;
using StoredProcedureToCsv.Faker;
using System.Data;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

#region Benchmark
//BenchmarkRunner.Run<CsvGeneratorBenchmarks>();
//return;
#endregion

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var configuration = builder.Build();

var connectionString = configuration.GetSection("ConnectionStrings:Default").Value;

//using var connection = new SqlConnection(connectionString);
//await connection.OpenAsync();

//var result = await connection.QueryAsync(
//    "CsvGetTickets",
//    commandType: CommandType.StoredProcedure
//    );

//var csv = CsvGenerator.ToCsvNuget(result);
//File.WriteAllText("tickets.csv", csv);
//File.WriteAllText("tickets.csv", BitConverter.ToString(csv));

//var result1 = await connection.QueryAsync(
//    "CsvGetFunds",
//    commandType: CommandType.StoredProcedure
//    );

//var csv1 = CsvGenerator.ToCsvNuget(result1);
//File.WriteAllText("funds.csv", csv1);

var fakeRecordsCount = 1000000;
var stopWatch = new Stopwatch();

Console.WriteLine("Generating fake tickets...");

var result = TicketFaker.GetFakeTickets(fakeRecordsCount);
stopWatch.Start();
//var csv = CsvGenerator.ToCsvNuget(result);
var csv = await CsvGenerator.ToCsvBytesAsync(result);
stopWatch.Stop();
Console.WriteLine("RunTime in seconds: " + stopWatch.Elapsed.TotalSeconds);
File.WriteAllBytes("tickets.csv", csv);

stopWatch.Reset();

Console.WriteLine("Generating fake funds...");

var result1 = FundTransactionFaker.GetFakeFundTransactions(fakeRecordsCount);
stopWatch.Start();
var csv1 = await CsvGenerator.ToCsvBytesAsync(result);
stopWatch.Stop();
Console.WriteLine("RunTime in seconds: " + stopWatch.Elapsed.TotalSeconds);
File.WriteAllBytes("funds.csv", csv1);

Console.ReadLine();

//for 2 milion records *writing to csv*  aproximately 7.5 seconds
//for 5 milion records *writing to csv*  aproximately 18 seconds
