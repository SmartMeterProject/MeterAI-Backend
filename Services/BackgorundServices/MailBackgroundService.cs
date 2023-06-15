using Business.Abstract;
using Scriban;
using Counter.Services.MailService;

namespace Counter.Services.BackgorundServices
{

    public class MailBackgroundService : IHostedService, IDisposable
    {
        private Timer _timer = null!;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailSender _emailSender;
        public MailBackgroundService(IServiceProvider serviceProvider, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            var bot = new Bot
            {
                BotId = 1,
                BotName = "ELTEK",
                User = "Admin",
                Tarih = DateTime.Now
            };

            var html = "";
            string path = "./wwwroot/htmlpage.html";
            using (StreamReader SourceReader = System.IO.File.OpenText(path))
            {
                html = await SourceReader.ReadToEndAsync();
            }

            var template = Template.Parse(@html, path);
            var emailBody = template.Render(new { bot = bot });
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IUserService>();
                var users = await context.GetUsersByClaim("Customer");
                _emailSender.SendEmailAsync(users,emailBody);
            }




            //var bulletinDailies = await _apiOperations.GetAll();

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
