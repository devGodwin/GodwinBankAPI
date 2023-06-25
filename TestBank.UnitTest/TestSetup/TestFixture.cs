using AutoMapper;
using GodwinBankAPI.Services.AccountServices;
using GodwinBankAPI.Services.AuthServices;
using GodwinBankAPI.Services.TransactionServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestBank.UnitTest.TestSetup;

public class TestFixture
{
    public ServiceProvider ServiceProvider { get; }
    
    public TestFixture()
    {
        var services = new ServiceCollection();
        ConfigurationManager.SetupConfiguration();

        services.AddSingleton(sp => ConfigurationManager.Configuration);

        services.AddLogging(x => x.AddConsole());
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IAuthServices, AuthServices>();
        services.AddScoped<IAccountServices, AccountServices>();
        services.AddScoped<ITransactionServices, TransactionServices>();

        ServiceProvider = services.BuildServiceProvider();
    }
}