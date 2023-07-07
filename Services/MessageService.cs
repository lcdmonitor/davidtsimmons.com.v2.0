using Services.Repositories;
using Contracts;

namespace Services;

public interface IMessageService
{
    public List<Message> GetAllMessages();
}

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository=messageRepository;
    }

    public List<Message> GetAllMessages()
    {
        return _messageRepository.GetMessages();
    }
}