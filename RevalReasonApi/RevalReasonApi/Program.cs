using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Utilities;
using Serilog;


namespace RevalReasonApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));
            builder.Services.AddControllers();
            builder.Services.AddTransient(connection => new AppDb(builder.Configuration["ConnectionStrings:ReasonConnection"], int.Parse(builder.Configuration["ConnectionStrings:CommandTimeout"])));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers().AddNewtonsoftJson();
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