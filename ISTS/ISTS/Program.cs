using IdentityServer4.EntityFramework.Mappers;
using ISTS.Application;
using ISTS.Infrastructure;
using ISTS.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add services to the container.
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();


builder.Services.AddControllersWithViews();

builder.Services.AddInfrastructure(builder.Configuration).AddIdentity(builder.Environment).AddRepositories();


SeedData.EnsureSeedData(builder.Configuration.GetConnectionString("IdentityDbConnection"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

await InitializeDatabase(app);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


async Task InitializeDatabase(IApplicationBuilder app)
{
    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
    {
        var persistedContext = serviceScope.ServiceProvider.GetRequiredService<IdentityServerPersistedGrantDbContext>();
        if (persistedContext.Database.IsSqlServer())
        {
            await persistedContext.Database.MigrateAsync();
        }

        var context = serviceScope.ServiceProvider.GetRequiredService<IdentityServerConfigurationDbContext>();

        if (context.Database.IsSqlServer())
        {
            await context.Database.MigrateAsync();
        }

        var identityContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (identityContext.Database.IsSqlServer())
        {
            await identityContext.Database.MigrateAsync();
        }

        if (Config.ApiClients.Any())
        {
            foreach (var client in Config.ApiClients)
            {
                if (!context.Clients.Any(x => x.ClientId == client.ClientId))
                {
                    context.Clients.Add(client.ToEntity());
                }
            }
        }

        if (Config.ApiResources.Any())
        {
            foreach (var apiResource in Config.ApiResources)
            {
                if (context != null && !context.ApiResources.Any(x => x.Name == apiResource.Name))
                {
                    context.ApiResources.Add(apiResource.ToEntity());
                }
            }
        }

        if (Config.IdentityResources.Any())
        {
            foreach (var resource in Config.IdentityResources)
            {
                if (context != null && !context.IdentityResources.Any(x => x.Name == resource.Name))
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
            }

        }

        if (Config.ApiScopes.Any())
        {
            foreach (var resource in Config.ApiScopes)
            {
                if (context != null && !context.ApiScopes.Any(x => x.Name == resource.Name))
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
            }
        }

        context?.SaveChanges();
    }
}