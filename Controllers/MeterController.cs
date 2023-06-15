using Business.Abstract;
using Counter.Provider;
using Entity.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Counter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MeterController : Controller
    {
        private readonly IMeterService _meterService;
        private readonly IUserProvider _userProvider;

        public MeterController(IMeterService meterService, IUserProvider userProvider)
        {
            _meterService = meterService;
            _userProvider = userProvider;
        }

        [Authorize]
        [HttpGet("getmeterinfo")]
        public async Task<GetMeterInfoResponse> GetMeterInfo()
        {
            var userId = _userProvider.GetUserId();
            var result = await _meterService.GetMeterInfo(Convert.ToInt32(userId));
            return result;
        }
    }
}
