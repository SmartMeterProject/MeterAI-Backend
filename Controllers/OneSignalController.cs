using Business.Abstract;
using Counter.Helper;
using Counter.Models;
using Counter.Provider;
using Entity.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Counter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OneSignalController : Controller
    {
        private readonly IUserOneSignalService _userOneSignalService;
        private readonly IConfiguration _configuration;
        private readonly IUserProvider _userProvider;



        public OneSignalController(IUserOneSignalService userOneSignalService, IConfiguration configuration, IUserProvider userProvider)
        {
            _userOneSignalService = userOneSignalService;
            _configuration = configuration;
            _userProvider = userProvider;
        }

        [HttpPost("SendNotificationAllUsers")]
        public async Task<IActionResult> SendNotificationAllUsers()
        {
            var oneSignalList = await _userOneSignalService.GetUserOneSignalsAsync();
            var oneSignalIds = oneSignalList.Select(x => x.OneSignalId).ToList();
            var oneSignalArray = oneSignalIds.ToArray();

            string appId = _configuration.GetSection(AppSettingKey.OneSignalAppId).Value;
            string restKey = _configuration.GetSection(AppSettingKey.OneSignalRestKey).Value;

            var options = new NotificationCreateOptions()
            {
                app_id = appId,
                include_player_ids = oneSignalArray,
                contents = new Dictionary<string, string> { { "en", "Merhaba!" } }
            };

            string result = await OneSignalHelper.OneSignalPushNotification(options, restKey);


            return Ok();
        }

        [Authorize]
        [HttpPost("UpdateDeviceId")]
        public async Task<IActionResult> UpdateDeviceId(UserOneSignalUpdateDto userOneSignalUpdateDto)
        {
            var userId = _userProvider.GetUserId();
            var result = await _userOneSignalService.UpdateUserOneSignalId(userOneSignalUpdateDto, Convert.ToInt32(userId));
            return Ok(result);
        }

    }
}
