using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RapidPay2.Extensions;
using RapidPay2.Infrastructure;
using RapidPay2.Infrastructure.SQLServer;
using RapidPay2.Middleware;
using RapidPay2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerConfig();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var databaseType = builder.Configuration["DATABASE_TYPE"];
if (databaseType == "dynamodb")
{
    builder.Services.AddSingleton<IAmazonDynamoDB>(_ =>
    {
        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = builder.Configuration["DYNAMODB_URL"]
        };
        return new AmazonDynamoDBClient(config);
    });
    builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
    builder.Services.AddHostedService<DynamoDBInit>();
    builder.Services.AddScoped<ICardsRepository, RapidPay2.Infrastructure.DynamoDB.CardsRepository>();
}
else
{
    builder.Services.AddDbContext<CardsContext>(options =>
        options.UseSqlServer(builder.Configuration["SQLSERVER_CONNECTION_STRING"]));
    builder.Services.AddHostedService<SqlServerInit>();
    builder.Services.AddScoped<ICardsRepository, RapidPay2.Infrastructure.SQLServer.CardsRepository>();
}

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICardGenerator, CardGenerator>();
builder.Services.AddScoped<IPaymentFeesModule, PaymentFeesModule>();
builder.Services.AddScoped<ICardManagementService, CardManagementService>();


builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "rapidpay",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AUTH_KEY"]!))
        };
    })
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddAuthorization(options =>
{
    var bearerPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .Build();
    options.DefaultPolicy = bearerPolicy;
    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, bearerPolicy);
    options.AddPolicy("BasicAuthentication", policyBuilder => policyBuilder
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes("BasicAuthentication")
        .Build());
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

var _ = UniversalFeesExchange.Instance;
await app.RunAsync();
