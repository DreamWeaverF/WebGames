namespace Dreamwear
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();
            var app = builder.Build();
            app.UseWebSockets();
            app.MapControllers();
            app.Run("http://127.0.0.1:50001");
        }
    }
}
