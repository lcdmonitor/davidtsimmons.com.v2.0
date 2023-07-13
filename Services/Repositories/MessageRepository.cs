using MySqlConnector;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Contracts.Authentication;
using Contracts.Message;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace Services.Repositories;

[ScopedService]
public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetMessagesAsync();
}

public class MessageRepository : IMessageRepository
{
    public IConfiguration Configuration { get; }
    private readonly ILogger<MessageRepository> _logger;

    public MessageRepository(IConfiguration configuration, ILogger<MessageRepository> logger)
    {
        Configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync()
    {
        _logger.LogInformation("Getting Messages");

        IEnumerable<Message> messages;

        using (var connection = new MySqlConnection(Configuration.GetConnectionString(ConnectionStrings.MySqlConnectionStringSection)))
        {
            await connection.OpenAsync();
            messages = await connection.QueryAsync<Message>("Select Id, MessageText from Messages");
        }

        return messages;
    }
}