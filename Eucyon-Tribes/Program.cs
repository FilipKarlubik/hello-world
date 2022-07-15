using Microsoft.EntityFrameworkCore;
using Eucyon_Tribes.Context;
using Eucyon_Tribes.Services;
using Serilog;
using Eucyon_Tribes.Factories;
using Microsoft.OpenApi.Models;
using Eucyon_Tribes.Extensions;
using Eucyon_Tribes.Config;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();

if (env.Equals("Development"))
{
    builder.Services.AddDbContext<ApplicationContext>(dbBuilder => dbBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IAuthService, JWTService>();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Eucyon Tribes API",
        Description = "An ASP.NET Core Web API for online game Eucyon Tribes"
    });
});
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IBuildingFactory, BuildingFactory>();
builder.Services.AddTransient<IBuildingService, BuildingService>();
builder.Services.AddTransient<IResourceFactory, ResourceFactory>();
builder.Services.AddTransient<IKingdomFactory, KingdomFactory>();
builder.Services.AddTransient<IArmyFactory, ArmyFactory>();
builder.Services.AddTransient<IArmyService, ArmyService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IKingdomService, KingdomService>();
builder.Services.AddTransient<ILeaderboardService, LeaderboardService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<ArmyServiceConfig>(s => new ArmyServiceConfig()
{
    ArmySizeLimit = 12
}
);


var app = builder.Build();

app.ConfigureExceptionHandler();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = String.Empty;
    });
}

app.Run();

public partial class Program { }