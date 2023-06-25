using System.Security.Claims;
using System.Text;
using Elasticsearch.Net;
using GodwinBankAPI.ElasticSearch;
using GodwinBankAPI.Helper;
using GodwinBankAPI.Model.AccountModel.ResponseModel;
using GodwinBankAPI.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Nest;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GodwinBankAPI.ServicesExtension;

public static class ServicesExtension
{
    public static void AddBearerAuthentication( this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.AddAuthentication(x =>
            { 
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = async context =>
                    {
                        await Task.Delay(0);
                           
                        string account = context.Principal?.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
                        var accountData = JsonConvert.DeserializeObject<AccountResponseModel>(account);

                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Email, JsonConvert.SerializeObject(accountData))
                        };

                        var appIdentity = new ClaimsIdentity(claims, CommonConstants.AppAuthIdentity);
                        context.Principal?.AddIdentity(appIdentity);
                    }
                };
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = configuration["BearerTokenConfig:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["BearerTokenConfig:Key"])),
                    ValidAudience = configuration["BearerTokenConfig:Audience"]
                };
            });
    }

    public static void AddRedisCache(this IServiceCollection services, Action<RedisConfig> redisConfig)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        if (redisConfig is null) throw new ArgumentNullException(nameof(redisConfig));

        services.Configure(redisConfig);
        var redisConfiguration = new RedisConfig();
        
        redisConfig.Invoke(redisConfiguration);

        var connectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions()
        {
            EndPoints = { redisConfiguration.BaseUrl },
            AllowAdmin = true,
            AbortOnConnectFail = false,
            ReconnectRetryPolicy = new LinearRetry(500),
            DefaultDatabase = redisConfiguration.Database
        });

        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        services.AddSingleton<IRedisService, RedisService>();

    }
    
    public static void AddElasticSearch(this IServiceCollection services, Action<ElasticSearchConfig> elasticSearchConfig)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        if (elasticSearchConfig is null) throw new ArgumentNullException(nameof(elasticSearchConfig));

        services.Configure(elasticSearchConfig);
        
        var elasticSearchConfiguration = new ElasticSearchConfig();
        
        elasticSearchConfig.Invoke(elasticSearchConfiguration);

        var pool = new SingleNodeConnectionPool(new Uri(elasticSearchConfiguration.BaseUrl));
        var connectionSettings = new ConnectionSettings(pool).DefaultIndex(elasticSearchConfiguration.AccountIndex);

        connectionSettings.PrettyJson();
        connectionSettings.DisableDirectStreaming();

        var elasticClient = new ElasticClient(connectionSettings);
        var elasticSearchLowLevelClient = new ElasticLowLevelClient(connectionSettings);

        services.AddSingleton<IElasticClient>(elasticClient);
        services.AddSingleton<IElasticLowLevelClient>(elasticSearchLowLevelClient);
        services.AddSingleton<IElasticSearchService, ElasticSearchService>();

    }
}