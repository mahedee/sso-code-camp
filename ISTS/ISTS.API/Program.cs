using FluentValidation.AspNetCore;
using ISTS.API.Filters;
using ISTS.API.Services;
using ISTS.Application;
using ISTS.Application.Common.Interfaces;
using ISTS.Application.Common.Models;
using ISTS.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();


builder.Services.AddControllers(options =>
        options.Filters.Add<ApiExceptionFilterAttribute>()
    )
    .ConfigureApiBehaviorOptions(opt =>
    {
        opt.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
        opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    })
    .AddFluentValidation(flv =>
    {
        flv.RegisterValidatorsFromAssemblyContaining<Program>();
        flv.DisableDataAnnotationsValidation = true;
        flv.AutomaticValidationEnabled = false;
    });


// Add services to the container.

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Environment)
    .AddRepositories();


builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

var identityConfiguration =
    builder.Configuration.GetSection("IdentityServerConfiguration").Get<IdentityServerConfiguration>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddIdentityServerAuthentication(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = identityConfiguration.Authority;
        options.RequireHttpsMetadata = identityConfiguration.RequireHttpsMetaData;
        options.ApiName = identityConfiguration.ApiName;
        options.RoleClaimType = "role";


        //TODO: This option must be removed from Production. This invoked to by pass SSL Certificate verification
        options.JwtBackChannelHandler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = delegate { return true; }
        };
    });
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
