namespace Product.Query.API;

public class ApplicationOptions
{
    public ConnectionString ConnectionStrings { get; set; }
}
public class ConnectionString
{
    public string MssqlDbConnectionString { get; set;}
    public string RedisConnectionString { get;}
}
