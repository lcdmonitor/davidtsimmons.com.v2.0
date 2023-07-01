using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using davidtsimmons.com.Models;

#region clean me up
using davidtsimmons.com.Repositories;
using Newtonsoft.Json;
#endregion

namespace davidtsimmons.com.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
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
            var messages = TestRepository.GetMessages();

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
