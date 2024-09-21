using BookLibrary.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.API.Services.BackgroundJob
{
    public class BookBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(10);

        public BookBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckForAvailableBooksAsync(stoppingToken);
                await CheckForExpiredReservationsAsync(stoppingToken);
                await Task.Delay(_checkInterval, stoppingToken); // Wait for the next interval
            }
        }

        private async Task CheckForAvailableBooksAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<EFContext>();

                var pendingNotification = await dbContext.Notification
                    .Where(x => !x.IsSent).ToListAsync(cancellationToken);

                foreach (var pending in pendingNotification)
                {
                    var availableBookList = await dbContext.BookRecord
                        .Where(x => x.BookId == pending.BookId && x.Status == BookStatus.Return).ToListAsync(cancellationToken);

                    foreach (var email in availableBookList)
                    {
                        // FUTURE TODO: Send email notification to users interested in those books
                        pending.Notified();
                        dbContext.Notification.Add(pending);
                    }
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task CheckForExpiredReservationsAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<EFContext>();

                var expiredReservations = await dbContext.Reservation
                    .Where(x => x.IsActive && x.ExpiresAt.Date >= DateTime.UtcNow.Date)
                    .ToListAsync(cancellationToken);

                foreach (var reservations in expiredReservations)
                {
                    // FUTURE TODO: Send email notification to users interested in those books
                    reservations.EndReservation();
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
