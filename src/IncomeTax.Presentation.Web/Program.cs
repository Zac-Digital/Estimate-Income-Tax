using IncomeTax.Application.Journey.Command;
using IncomeTax.Application.Journey.Query;
using IncomeTax.Application.Session;

namespace IncomeTax.Presentation.Web;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();

        builder.Services.AddDistributedMemoryCache(); // TODO: Look at Redis later
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        
        builder.Services.AddTransient<SessionService>();
        builder.Services.AddTransient<JourneyCommands>();
        builder.Services.AddTransient<JourneyQueries>();

        WebApplication app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            // TODO: Error Page
            // app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseSession();
        app.MapStaticAssets();
        app.MapRazorPages().WithStaticAssets();

        app.Run();
    }
}