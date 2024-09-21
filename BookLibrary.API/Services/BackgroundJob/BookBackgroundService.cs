using BookLibrary.Domain.DTOs;
using BookLibrary.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.API.Services.BackgroundJob
{
    public class BookBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

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

                // Fetch books that are no longer reserved or borrowed
                //var availableBooks = await dbContext.Book
                //    .Where(b => b.Status == BookStatus.Available && b.Reservations.Any())
                //    .ToListAsync(cancellationToken);

                //foreach (var book in availableBooks)
                //{
                //    var userToNotify = book.Reservations.Select(r => r.CustomerEmail).ToList();

                //    foreach (var email in userToNotify)
                //    {
                //        var notificationDto = new NotificationDto
                //        {
                //            BookId = book.Id,
                //            UserEmail = email,
                //            NotificationDate = DateTime.UtcNow,
                //            IsSent = false
                //        };

                //        dbContext.Notification.Add(notification);

                //        // Remove the reservation since the book is now available
                //        book.Reservations.Clear();
                //    }
                //}

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
                    // FUTURE TODO: Send email notification user interested in those books
                    reservations.EndReservation();
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
