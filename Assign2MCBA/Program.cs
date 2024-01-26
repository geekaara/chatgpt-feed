using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Assign2MCBA.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<DataService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<BankingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");
await GetData(app.Services);
app.Run();

async Task GetData(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    var context = serviceProvider.GetRequiredService<BankingContext>();
    var dataService = serviceProvider.GetRequiredService<DataService>();
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("DB is empty, fetching customers from API");
    await dataService.GetInitialDataAsync("https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/");
    logger.LogInformation("Customers fetched and database updated.");
    // try
    // {
    //     if (!context.Customers.Any())
    //     {
    //         logger.LogInformation("DB is empty, fetching customers from API");
    //         await dataService.GetInitialDataAsync("https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/");
    //         logger.LogInformation("Customers fetched and database updated.");
    //     }
    //
    // }
    // catch (Exception ex)
    // {
    //     logger.LogError(ex, "An error occurred while loading initial data.");
    // }
}
