using Contracts.Message;

namespace davidtsimmons.com.Models.HealthCheckModels
{
    public class Pong
    {
        public string Response { get {return "Pong";}}

        public IEnumerable<Message>? Messages {get; set;}

        public string? ClientIP {get; set;}
    }
}