using Microsoft.AspNetCore.Hosting;

namespace Exercise_4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .Build()
                .Run();
        }


    }
}
