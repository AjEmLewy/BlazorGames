namespace Gejms.Server.Utilities;

public static class StringUtilities
{
    public static string ParseDbURI(string url)
    {
        string temp = url.Replace("postgres://", "");
        string user = temp.Split("@")[0].Split(":")[0];
        string password = temp.Split("@")[0].Split(":")[1];

        string host = temp.Split("@")[1].Split("/")[0];
        string dbName = temp.Split("@")[1].Split("/")[1];

        string result = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};SSL Mode=Require;Trust Server Certificate=true;",
            host, user, password, dbName);
        return result;
    }
}
