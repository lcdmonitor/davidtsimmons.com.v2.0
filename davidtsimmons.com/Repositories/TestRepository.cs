using MySqlConnector;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace davidtsimmons.com.Repositories;

public class Message
{
    public int Id {get; set;}
    public string? MessageText {get; set;}
}

public class TestRepository
{
    public static List<Message> GetMessages()
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("appsettings.json");
        var configuration = builder.Build();
        var connection = new MySqlConnection(configuration.GetConnectionString("Default"));

        var messages = connection.Query<Message>("Select Id, MessageText from messages");

        return messages.ToList<Message>();
    }
}