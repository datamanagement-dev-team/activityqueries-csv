public class DbRequest
{
    public string StoredProcedureName { get; set; }
    public object? Parameters { get; set; }

    public DbRequest()
    {
        StoredProcedureName = string.Empty;
    }

}