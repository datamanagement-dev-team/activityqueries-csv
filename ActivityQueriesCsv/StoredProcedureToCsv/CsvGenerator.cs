using CsvHelper;
using System.Dynamic;
using System.Globalization;
using System.Text;

namespace StoredProcedureToCsv
{
    public static class CsvGenerator
    {
        #region Custom csv implementaion
        private static string ToCsv(IEnumerable<dynamic> rows)
        {
            var sb = new StringBuilder();
            using var enumerator = rows.GetEnumerator();

            // No rows
            if (!enumerator.MoveNext())
                return "";

            // First row determines the column structure
            var firstRow = (IDictionary<string, object>)enumerator.Current;
            var headers = firstRow.Keys.ToList();

            // Write header row
            sb.AppendLine(string.Join(",", headers));

            // Write first row
            sb.AppendLine(string.Join(",", headers.Select(h => EscapeCsv(firstRow[h]))));

            // Write remaining rows
            while (enumerator.MoveNext())
            {
                var row = (IDictionary<string, object>)enumerator.Current;
                sb.AppendLine(string.Join(",", headers.Select(h => EscapeCsv(row[h]))));
            }

            return sb.ToString();
        }

        private static string EscapeCsv(object value)
        {
            if (value == null) 
                return "";

            var str = value.ToString();
            if (str.Contains(",") || str.Contains("\"") || str.Contains("\n"))
            {
                str = "\"" + str.Replace("\"", "\"\"") + "\"";
            }
            return str;
        }
        #endregion

        public static string ToCsvNuget(IEnumerable<dynamic> records)
        {
            using var writer = new StringWriter();
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(records);
            var result = writer.ToString();
            return result;
        }

        public static async Task<byte[]> ToCsvBytesAsync<T>(IEnumerable<T> records)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: false);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await csv.WriteRecordsAsync(records);
            await writer.FlushAsync();

            return memoryStream.ToArray();
        }
    }
}
