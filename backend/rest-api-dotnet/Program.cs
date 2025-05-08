using Microsoft.EntityFrameworkCore;
using RestApiDotNet.Model.Context;
using RestApiDotNet.Business;
using RestApiDotNet.Business.Implementations;
using RestApiDotNet.Repository;
using EvolveDb;
using Serilog;
using MySqlConnector;
using RestApiDotNet.Model.Repository.Generic;
using Microsoft.Net.Http.Headers;
using RestApiDotNet.HyperMedia.Filters;
using RestApiDotNet.HyperMedia.Enricher;
using RestApiDotNet.Hypermedia.Enricher;
using RestApiDotNet.Services;
using RestApiDotNet.Services.Implementations;
using RestApiDotNet.Model.Repository;
using RestApiDotNet.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Adiciona IHttpClientFactory
builder.Services.AddHttpClient();

// Lowercase Endpoints
builder.Services.AddRouting(option => option.LowercaseUrls = true);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Authentication
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IAuthService, GoogleOAuthService>();

// Dependency Injection
builder.Services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
builder.Services.AddScoped<IBookBusiness, BookBusinessImplementation>();
builder.Services.AddScoped<ILoginBusiness, LoginBusinessImplementation>();
builder.Services.AddScoped<IFileBusiness, FileBusinessImplementation>();

// Generic repository pattern
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(
    connection,
    new MySqlServerVersion(new Version(9, 3, 0)))
);

//if (builder.Environment.IsDevelopment())
//{
//    MigrateDatabase(connection);
//}

builder.Services.AddMvcCore(options =>
{
    options.RespectBrowserAcceptHeader = true;

    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
    options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
}).AddXmlSerializerFormatters();

// HATEOAS
var filterOptions = new HyperMediaFilterOptions();
filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
filterOptions.ContentResponseEnricherList.Add(new BookEnricher());
builder.Services.AddSingleton(filterOptions);

// API versioning
builder.Services.AddApiVersioning();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Rest API .Net Core",
        Version = "v1",
        Description = "API Restful",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Guilherme Fabris",
            Url = new Uri("https://github.com/gfabrissouza")
        }
    });
});

// Get configurations from appsettings.json
var allowedOrigins = builder.Configuration
    .GetSection("CorsConfiguration:AllowedOrigins")
    .Get<string[]>();

if (allowedOrigins is null || allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("CORS origins not configured.");
}

var authConfiguration = new AuthConfiguration();

new ConfigureFromConfigurationOptions<AuthConfiguration>(
    builder.Configuration.GetSection("AuthConfiguration"))
    .Configure(authConfiguration);

builder.Services.AddSingleton(authConfiguration);

var tokenConfiguration = new TokenConfiguration();

new ConfigureFromConfigurationOptions<TokenConfiguration>(
    builder.Configuration.GetSection("TokenConfiguration"))
    .Configure(tokenConfiguration);

builder.Services.AddSingleton(tokenConfiguration);

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenConfiguration.Issuer,
        ValidAudience = tokenConfiguration.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret)),
    };

    // Reads token from cookie instead Authorization header
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("access_token", out var token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());
});

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", builder =>
    {
        builder.WithOrigins(allowedOrigins)
               .AllowCredentials()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

//app.UseHttpsRedirection();

// Servers static files in wwwroot directory
//app.UseDefaultFiles();
//app.UseStaticFiles();

app.UseRouting();

app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(app =>
{
    app.SwaggerEndpoint("v1/swagger.json", "Rest API .Net Core v1");
    app.RoutePrefix = "swagger";
});

//var option = new RewriteOptions().AddRedirect("^$", "index.html");
//app.UseRewriter(option);

app.MapControllers();
app.MapControllerRoute("DefaultApi", "{controller=values}/v{version=apiVersion}/{id?}");

app.Run();

static void MigrateDatabase(string connection)
{
    try
    {
        var evolveConnection = new MySqlConnection(connection);
        var evolve = new Evolve(evolveConnection, Log.Information)
        {
            Locations = ["Database/Migrations", "Database/Dataset"],
            IsEraseDisabled = true,
            CommandTimeout = 60
        };
        evolve.Migrate();
    }
    catch (Exception ex)
    {
        throw;
    }
}