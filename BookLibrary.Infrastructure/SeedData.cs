using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Infrastructure
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EFContext(serviceProvider.GetRequiredService<DbContextOptions<EFContext>>()))
            {
                bool dataAdded = false;
                if (!context.User.Any())
                {
                    var userInst = new User();
                    var userDto = new UserDto
                    {
                        Email = "admin@example.com",
                        Name = "Admin",
                        Role = UserRole.Admin
                    };
                    var user = userInst.AddUser(userDto);
                    user.SetPassword("Password123");

                    context.User.Add(user);
                    dataAdded = true;
                }

                if (!context.Book.Any())
                {
                    BookDto[] bookCollection =
                    {
                        new()
                        {
                            Title = "Good to Great: Why Some Companies Make the Leap... and Others Don't",
                            Authors = "Jim Collins",
                            CoverPictureUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1546097703i/76865.jpg",
                            DatePublished = DateTime.Parse("2001-11-01"),
                            Genre = BookGenre.Business,
                            Description = @"To find the keys to greatness, Collins's 21-person research team read and coded 6,000 articles, generated more than 2,000 pages of interview transcripts and created 384 megabytes of computer data in a five-year project. The findings will surprise many readers and, quite frankly, upset others."
                        },
                        new()
                        {
                            Title = "The Richest Man in Babylon",
                            Authors = "George S. Clason",
                            CoverPictureUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1543897827i/43097201.jpg",
                            DatePublished = DateTime.Parse("1926-01-01"),
                            Genre = BookGenre.Finance,
                            Description = @"The Richest Man in Babylon is an early twentieth century classic about financial investment and monetary success. Through a series of enlightening parables set in the heart of ancient Babylon, Clason provided his readers with economic tips and tools for financial success. Here his text is interpreted for today and offers you 52 simple, powerful and proven techniques to manage your finances. Karen McCreadies interpretation of Clason's work illustrates the timeless nature of his insights by bringing them to life through modern case studies."
                        },
                        new()
                        {
                            Title = "The 48 Laws of Power",
                            Authors = "Robert Greene",
                            CoverPictureUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1694722764i/1303.jpg",
                            DatePublished = DateTime.Parse("1998-01-01"),
                            Genre = BookGenre.Business,
                            Description = @"In the book that People magazine proclaimed “beguiling” and “fascinating,” Robert Greene and Joost Elffers have distilled three thousand years of the history of power into 48 essential laws by drawing from the philosophies of Machiavelli, Sun Tzu, and Carl Von Clausewitz and also from the lives of figures ranging from Henry Kissinger to P.T. Barnum."
                        },
                        new()
                        {
                            Title = "How to Win Friends and Influence People",
                            Authors = "Dale Carnegie",
                            CoverPictureUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1442726934i/4865.jpg",
                            DatePublished = DateTime.Parse("1936-10-01"),
                            Genre = BookGenre.Business,
                            Description = @"You can go after the job you want...and get it! You can take the job you have...and improve it! You can take any situation you're in...and make it work for you!

                                          Since its release in 1936, How to Win Friends and Influence People has sold more than 30 million copies. Dale Carnegie's first book is a timeless bestseller, packed with rock-solid advice that has carried thousands of now famous people up the ladder of success in their business and personal lives."
                        }
                    };

                    var bookList = new List<Book>();
                    var bookInst = new Book();
                    for (int i = 0; i < bookCollection.Length; i++)
                    {
                        var book = bookInst.AddBook(bookCollection[i]);
                        bookList.Add(book);
                    }

                    context.Book.AddRange(bookList);
                    dataAdded = true;
                }

                if (!dataAdded)
                {
                    return;   // DB has been seeded
                }

                context.SaveChanges();
            }
        }
    }
}