using BenchmarkDotNet.Attributes;
using StoredProcedureToCsv.Faker;

namespace StoredProcedureToCsv
{
    [MemoryDiagnoser]
    public class CsvGeneratorBenchmarks
    {
        private IEnumerable<dynamic> tickets;
        private int fakeRecordsCount = 5000000;

        [GlobalSetup]
        public void Setup()
        {
            tickets = TicketFaker.GetFakeTickets(fakeRecordsCount);
        }

        [Benchmark]
        public async Task<byte[]> ToCsvBytesAsync()
        {
            return await CsvGenerator.ToCsvBytesAsync(tickets);
        }
    }
}
