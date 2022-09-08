using Microsoft.EntityFrameworkCore;
using Eucyon_Tribes.Context;
using Eucyon_Tribes.Services;
using Serilog;
using Eucyon_Tribes.Factories;
using Microsoft.OpenApi.Models;
using Eucyon_Tribes.Extensions;
using Tribes.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
MapSecretsToEnvVariables();

string connectionString = builder.Configuration.GetConnectionString("AzureSql");
var connectionBuilder = new SqlConnectionStringBuilder(connectionString);
connectionBuilder.UserID = builder.Configuration["AzureUser"];
connectionBuilder.Password = builder.Configuration["AzureSqlPassword"];
connectionString = connectionBuilder.ConnectionString;

if (env != null && env.Equals("Development"))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.MSSqlServer(connectionString, autoCreateSqlTable: true, tableName: "Logs")
    .CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);
}

builder.Services.AddDbContext<ApplicationContext>(dbBuilder => dbBuilder.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IAuthService, JWTService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TokenGenerationKey")))
        };
    });
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Tribes API",
        Description = "Test Api/Mvc Endpoints"
    });
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert Token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }, Array.Empty<string>()
                }
                });
});
builder.Services.AddTransient<IBuildingFactory, BuildingFactory>();
builder.Services.AddTransient<IBuildingService, BuildingService>();
builder.Services.AddTransient<IResourceFactory, ResourceFactory>();
builder.Services.AddTransient<IKingdomFactory, KingdomFactory>();
builder.Services.AddTransient<IArmyFactory, ArmyFactory>();
builder.Services.AddTransient<IArmyService, ArmyService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IKingdomService, KingdomService>();
builder.Services.AddTransient<ILeaderboardService, LeaderboardService>();
builder.Services.AddTransient<IPurchaseService, PurchaseService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IWorldService, WorldService>();
builder.Services.AddTransient<IBattleService, BattleService>();
builder.Services.AddHostedService<TimeService>();
builder.Services.AddTransient<RuleService, ConfigRuleService>();
builder.Services.AddTransient<ISoldierService, SoldierService>();
builder.Services.AddTransient<IBattleFactory, BattleFactory>();
builder.Services.AddTransient<ITwoStepAuthService, TwoStepAuthService>();
builder.Services.AddTransient<IResourceService, ResourceService>();

var app = builder.Build();

app.ConfigureExceptionHandler();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());
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

static void MapSecretsToEnvVariables()
{
    var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
    foreach (var child in config.GetChildren())
    {
        Environment.SetEnvironmentVariable(child.Key, child.Value);
    }
}

public partial class Program { }