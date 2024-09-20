using BookLibrary.API.Models;
using BookLibrary.API.Services.Books;
using BookLibrary.API.Services.Users;
using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.Aggregates.BookRecordAggregate;
using BookLibrary.Domain.Aggregates.ReservationAggregate;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.SeedWork;
using BookLibrary.Infrastructure;
using BookLibrary.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BookLibrary.API.Extensions
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(BaseRepository<>))
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IBookRepository, BookRepository>()
                .AddScoped<IReservationRepository, ReservationRepository>()
                .AddScoped<IBookRecordRepository, BookRecordRepository>();
        }

        //public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        //{
        //    return services
        //        .AddScoped<IUnitOfWork>();
        //}

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<EFContext>(options =>
                     options.UseNpgsql(configuration.GetConnectionString("BookLibraryDb")));
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IBookService, BookService>()
                .AddScoped<IUserService, UserService>();
        }
        
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Book Library",
                        Version = "v1",
                        Contact = new OpenApiContact { Name = "Book" }
                    });
                    //c.AddFluentValidationRules();
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BookLibrary.Api.xml"));
                });
        }
        
        public static IServiceCollection AddFluentValidators(this IServiceCollection services)
        {
            return services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<AddBookRequestValidator>();
        }

    }
}
