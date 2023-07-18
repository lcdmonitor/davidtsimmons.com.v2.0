using davidtsimmons.com.Models.HealthCheckModels;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace davidtsimmons.com.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly ILogger<HealthCheckController> _logger;
        private readonly IMessageService _messageService;

        public HealthCheckController(ILogger<HealthCheckController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        [HttpGet("Ping")]
        public async Task<ActionResult<Pong>> Ping() ///api/healthcheck/ping
        {
            _logger.LogInformation("Ping received");

            //load balancer support
            string forwardedFor = Request.HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            string clientIP = Request.HttpContext.Connection.RemoteIpAddress is not null ? Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : String.Empty;

            var remoteIpAddress = string.IsNullOrEmpty(forwardedFor) ? clientIP : forwardedFor;

            return new JsonResult(new Pong() { ClientIP = remoteIpAddress is not null ? remoteIpAddress : String.Empty, Messages = await _messageService.GetAllMessagesAsync() });
        }
    }
}