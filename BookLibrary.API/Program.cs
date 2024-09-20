
using BookLibrary.API.Extensions;
using BookLibrary.API.Middleware;

namespace BookLibrary.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddBusinessServices();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddFluentValidators();
            builder.Services.AddSwagger();

            var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
