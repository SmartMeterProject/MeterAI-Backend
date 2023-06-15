using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Counter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewController : Controller
    {
        private readonly INewService _newService;

        public NewController(INewService newService)
        {
            _newService = newService;
        }

        [Authorize]
        [HttpGet("getallnews")]
        public async Task<IActionResult> GetAllNews()
        {
            var result = await _newService.GetAllNews();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else { return BadRequest(); }

        }
    }
}
