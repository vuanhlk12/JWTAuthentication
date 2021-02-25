using Microsoft.Extensions.Configuration;
using System.IO;

namespace JWTAuthentication.Authentication
{
    public static class GlobalSettings
    {
        public static IConfiguration Config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

        public static string ConnectionStr = Config["ConnectionStrings:ConnStr"];
    }
}
