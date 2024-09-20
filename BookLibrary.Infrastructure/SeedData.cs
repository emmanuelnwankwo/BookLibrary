//using BookLibrary.Domain.Aggregates.BookAggregate;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BookLibrary.Infrastructure
//{
//    public static class SeedData
//    {
//        public static void Initialize(IServiceProvider serviceProvider)
//        {
//            using (var context = new EFContext(
//                serviceProvider.GetRequiredService<
//                    DbContextOptions<EFContext>>()))
//            {
//                if (context.Database.Book.Any())
//                {
//                    return;   // DB has been seeded
//                }
//                context.Book.AddRange(
//                    new Book
//                    {
//                        Title = "When Harry Met Sally"
//                    },
//                    new Book
//                    {
//                        Title = "Ghostbusters "
//                    }
//                );
//                context.SaveChanges();
//            }
//        }
//    }
//}