
using Authentication.Adapter.Configurations;
using Authentication.Adapter.Extensions;
using Marraia.Notifications.Configurations;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var redisConnection = builder.Configuration.GetSection("Redis:Configuration").Value
    ?? throw new InvalidOperationException("Redis configuration is not set");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = builder.Configuration.GetSection("Redis:InstanceName").Value;
    options.Configuration = builder.Configuration.GetSection("Redis:Configuration").Value;
});

// builder.Services.AddScoped<IProductAppService, ProductAppService>();
// builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
            builder =>
            builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
});

#region JWT
// Token and JWT service with Authentication Adapter
var tokenConfigurations = new TokenConfigurations();
new ConfigureFromConfigurationOptions<TokenConfigurations>(builder.Configuration.GetSection("TokenConfigurations"))
        .Configure(tokenConfigurations);

builder.Services.AddJwtSecurity(tokenConfigurations);
#endregion

builder.Services.AddSmartNotification();

// new RootBootstrapper().BootstrapperRegisterServices(builder.Services, builder.Configuration);
var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();