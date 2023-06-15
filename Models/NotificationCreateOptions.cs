namespace Counter.Models
{
    public class NotificationCreateOptions
    {
        public string app_id { get; set; }
        public string[] include_player_ids { get; set; }
        public Dictionary<string, string> contents { get; set; }
    }
}
