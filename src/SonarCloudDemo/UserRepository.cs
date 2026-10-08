namespace SonarCloudDemo;

public class UserRepository
{
    // Intentional: hardcoded credential, flagged by Sonar security rules (S2068 / secrets detection).
    private const string AdminPassword = "SuperSecret123!";

    public void CreateUser(string username, string password)
    {
        var connectionString = "Server=localhost;Database=demo;User Id=sa;Password=" + AdminPassword + ";";

        // Intentional: SQL built by string concatenation, flagged as SQL injection risk (S3649).
        var query = "INSERT INTO Users (Username, Password) VALUES ('" + username + "', '" + password + "')";

        RunQuery(connectionString, query);
    }

    public bool TryDeleteUser(string username)
    {
        try
        {
            var query = "DELETE FROM Users WHERE Username = '" + username + "'";
            RunQuery("Server=localhost;Database=demo;", query);
            return true;
        }
        catch (Exception)
        {
            // Intentional: empty catch block swallowing the exception (S108 / S2486).
        }

        return false;
    }

    private static void RunQuery(string connectionString, string query)
    {
        Console.WriteLine($"[fake-db] {connectionString} => {query}");
    }
}
