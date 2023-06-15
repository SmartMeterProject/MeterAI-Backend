using System.Collections;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Counter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IUserService _userService;


        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get(string claim)
        {

            var users = await _userService.GetUsersByClaim("Customer");
            return Ok(users);
        }
    }
}