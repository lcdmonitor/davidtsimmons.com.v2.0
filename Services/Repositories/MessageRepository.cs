using MySqlConnector;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Contracts;

namespace Services.Repositories;
public interface IMessageRepository
{
    public List<Message> GetMessages();
}

public class MessageRepository : IMessageRepository
{
    public IConfiguration Configuration { get; }  
    private readonly ILogger<MessageRepository> _logger;  

    public MessageRepository(IConfiguration configuration, ILogger<MessageRepository> logger)
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