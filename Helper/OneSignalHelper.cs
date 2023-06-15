using Counter.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Counter.Helper
{
    public class OneSignalHelper
    {
        public static async Task<string> OneSignalPushNotification(NotificationCreateOptions options, string restKey)
        {

            using var client = new HttpClient();

            string requestBody = JsonConvert.SerializeObject(options);

            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string authToken = restKey;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var response = await client.PostAsync("https://onesignal.com/api/v1/notifications", content);

            return "Ok";
        }
    }
}
