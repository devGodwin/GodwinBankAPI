using Microsoft.Extensions.Configuration;

namespace TestBank.UnitTest.TestSetup;

public static class ConfigurationManager
{
    public static IConfiguration Configuration { get; private set; }
    
    public static void SetupConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        Configuration = builder.Build();
    }
}