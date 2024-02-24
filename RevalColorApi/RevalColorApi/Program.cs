using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Utilities;
using Serilog;

namespace RevalColorApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<Appsetting>(builder.Configuration.GetSection("AppSettings"));
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddTransient(connection => new AppDb(builder.Configuration["ConnectionStrings:ColorConnection"], int.Parse(builder.Configuration["ConnectionStrings:CommandTimeout"])));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<General>(sp =>
            {
                var logTypeId = sp.GetRequiredService<IConfiguration>()["appSettings:LogTypeId"];
                return new General(short.Parse(logTypeId));
            });
            var app = builder.Build();
            try
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

            
            }
            catch (Exception ex)
            {
                // Log the exception if an error occurs during logger setup
                Console.WriteLine($"Error setting up Serilog: {ex}");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}