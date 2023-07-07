using MySqlConnector;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Contracts;

namespace Services.Repositories;
public interface ITestRepository
{
    public List<Message> GetMessages();
}

public class TestRepository : ITestRepository
{
    public IConfiguration Configuration { get; }  
    private readonly ILogger<TestRepository> _logger;  

    public TestRepository(IConfiguration configuration, ILogger<TestRepository> logger)
    {
        Configuration=configuration;
        _logger=logger;
    }

    public List<Message> GetMessages()
    {
        _logger.LogInformation("Getting Messages");
        
        var connection = new MySqlConnection(Configuration.GetConnectionString(ConnectionStrings.MySqlConnectionStringSection));

        var messages = connection.Query<Message>("Select Id, MessageText from messages");

        return messages.ToList<Message>();
    }
}