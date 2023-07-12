using Services.Repositories;
using Contracts;
using Microsoft.Extensions.Logging;
using Contracts.Message;

namespace Services;

public interface IMessageService
{
    public Task<IEnumerable<Message>> GetAllMessagesAsync();
}

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly ILogger<MessageService> _logger;
    public MessageService(IMessageRepository messageRepository, ILogger<MessageService> logger)
    {
        _messageRepository=messageRepository;
        _logger=logger;
    }

    public Task<IEnumerable<Message>> GetAllMessagesAsync()
    {
        return _messageRepository.GetMessagesAsync();
    }
}