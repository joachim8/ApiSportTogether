namespace ApiSportTogether.Services
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public NotificationBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();

                    // Appeler le service pour notifier les participants
                    await notificationService.NotifierParticipantsAsync();
                }

                // Attendre 24 heures avant de réexécuter
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }

}
