using System.Reflection;
using AutoMapper;
using GodwinBankAPI.Configurations;
using GodwinBankAPI.Data.AccountData;
using GodwinBankAPI.Data.TransactionData;
using GodwinBankAPI.ElasticSearch;
using GodwinBankAPI.Redis;
using GodwinBankAPI.Services.AccountServices;
using GodwinBankAPI.Services.AuthServices;
using GodwinBankAPI.Services.EmailServices;
using GodwinBankAPI.Services.TransactionServices;
using GodwinBankAPI.ServicesExtension;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AccountContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDb")));

builder.Services.AddDbContext<TransactionContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDb")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<ITransactionServices, TransactionServices>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddRedisCache(c=>builder.Configuration.GetSection(nameof(RedisConfig)).Bind(c));

builder.Services.Configure<BearerTokenConfig>(c => builder.Configuration.GetSection(nameof(BearerTokenConfig)).Bind(c));
builder.Services.Configure<EmailConfig>(c => builder.Configuration.GetSection(nameof(EmailConfig)).Bind(c));
 
builder.Services.AddBearerAuthentication(builder.Configuration);

builder.Services.AddElasticSearch(c=>builder.Configuration.GetSection(nameof(ElasticSearchConfig)).Bind(c));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x=> 
{
    x.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Godwin Bank Api",
        Version = "v1",
        Description = "Godwin Bank Api",
        Contact = new OpenApiContact()
        {
            Name = "Godwin Mensah",
            Email = "godwinmensah945@gmail.com"
        }
    });
    x.ResolveConflictingActions(resolver=>resolver.First());
    x.EnableAnnotations();
    x.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme()
    {
        Description = "Provide bearer token to access endpoints",
        Name = "Authorisation",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Scheme = "OAuth",
                Name = "Bearer",
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x=>x.SwaggerEndpoint("/swagger/v1/swagger.json","Godwin Bank Api"));
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();