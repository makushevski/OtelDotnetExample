namespace OtelDotnetExample.Api
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.WebHost.UseUrls("http://localhost:5001");

            var app = builder.Build();
            
            app.MapControllers();
            app.Run();
        }
    }
}