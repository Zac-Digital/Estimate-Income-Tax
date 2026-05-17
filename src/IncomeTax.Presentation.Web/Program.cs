using System.Diagnostics.CodeAnalysis;
using IncomeTax.Application.Journey;
using IncomeTax.Application.Session;
using IncomeTax.Presentation.Web.Filters;
using Microsoft.AspNetCore.HttpOverrides;

namespace IncomeTax.Presentation.Web;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages()
            .AddMvcOptions(options => options.Filters.Add<JourneyFilter>());

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.MaxAge = TimeSpan.FromMinutes(10);
        });

        builder.Services.AddTransient<SessionService>();
        builder.Services.AddTransient<JourneyValidator>();

        WebApplication app = builder.Build();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
            { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
        app.UseHsts();
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseSession();
        app.MapStaticAssets();
        app.MapRazorPages().WithStaticAssets();

        app.Run();
    }
}