namespace IncomeTax.Presentation.Web;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();

        WebApplication app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            // TODO: Error Page
            // app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.MapStaticAssets();
        app.MapRazorPages().WithStaticAssets();

        app.Run();
    }
}