using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using davidtsimmons.com.Models;
using Newtonsoft.Json;
using Services;

namespace davidtsimmons.com.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMessageService _messageService;

    public HomeController(ILogger<HomeController> logger, IMessageService messageService)
    {
        _logger = logger;
        _messageService = messageService;
    }

    public IActionResult Index()
    {
        #region Test Session
        _logger.LogInformation("Testing Session: " + HttpContext.Session.Id);

        var value = $"Session written at {DateTime.UtcNow.ToString()}";
        HttpContext.Session.SetString("Test", value);
        #endregion

        #region mysql test
        try
        {
            var messages = _messageService.GetAllMessages();

            _logger.LogInformation(JsonConvert.SerializeObject(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error Getting Messages");
        }
        #endregion

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
