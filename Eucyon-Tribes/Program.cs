using Microsoft.EntityFrameworkCore;
using Eucyon_Tribes.Context;
using Eucyon_Tribes.Services;
using Serilog;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Extensions;

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



var app = builder.Build();

app.ConfigureExceptionHandler();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }