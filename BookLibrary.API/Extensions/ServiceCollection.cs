﻿using BookLibrary.API.Models;
using BookLibrary.API.Models.Users;
using BookLibrary.API.Services.BackgroundJob;
using BookLibrary.API.Services.Books;
using BookLibrary.API.Services.Users;
using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.Aggregates.BookRecordAggregate;
using BookLibrary.Domain.Aggregates.NotificationAggregate;
using BookLibrary.Domain.Aggregates.ReservationAggregate;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.SeedWork;
using BookLibrary.Domain.Shared;
using BookLibrary.Infrastructure;
using BookLibrary.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

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
                .AddScoped<IBookRecordRepository, BookRecordRepository>()
                .AddScoped<INotificationRepository, NotificationRepository>();
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
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAuthService, AuthService>();
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
                    //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BookLibrary.Api.xml"));
                    c.IncludeXmlComments(GetXmlCommentPath());
                });
        }

        public static IServiceCollection AddFluentValidators(this IServiceCollection services)
        {
            return services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<AddBookRequestValidator>()
                .AddValidatorsFromAssemblyContaining<AddUserRequestValidator>()
                .AddValidatorsFromAssemblyContaining<BorrowBookRequestValidator>()
                .AddValidatorsFromAssemblyContaining<ReturnBookRequestValidator>()
                .AddValidatorsFromAssemblyContaining<LoginRequestValidator>()
                .AddValidatorsFromAssemblyContaining<NotifyBookRequestValidator>();
        }

        public static IServiceCollection AddAppSetting(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<GeneralConfig>(configuration.GetSection("GeneralConfig"));
        }

        public static IServiceCollection AddBackgroundService(this IServiceCollection services)
        {
            return services.AddHostedService<BookBackgroundService>();
        }

        public static IConfigurationBuilder AddEnvironment(this WebApplicationBuilder builder)
        {
            var environment = builder.Environment;
            return builder.Configuration
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();
        }

        static string GetXmlCommentPath()
        {
            var basePath = AppContext.BaseDirectory;
            var fileName = typeof(Program).GetTypeInfo().Assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }
    }
}
