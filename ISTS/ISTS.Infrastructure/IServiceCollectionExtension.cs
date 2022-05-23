using IdentityServer4.EntityFramework.Storage;
using ISTS.Application.Common.Constants;
using ISTS.Application.Common.Interfaces.DbContexts;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Core.Entities.Identity;
using ISTS.Infrastructure.DbContexts;
using ISTS.Infrastructure.Identity;
using ISTS.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;

namespace ISTS.Infrastructure
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            RegisterDbContexts<ApplicationDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext>(services, configuration, env);
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }

        // Need to discuss with Emrul about certification creation
        public static IServiceCollection AddIdentity(this IServiceCollection services, IWebHostEnvironment env)
        {
            X509Certificate2? cert = null;
            using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                certStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    // Replace below with your cert's thumbprint
                    "D39BDBF37A10EF291AB2C46521426D0BB09662D9",
                    false);
                // Get the first cert with the thumbprint
                if (certCollection.Count > 0)
                {
                    cert = certCollection[0];
                }
            }

            // Fallback to local file for development
            if (cert == null)
            {
                //cert = new X509Certificate2(Path.Combine(env.ContentRootPath, "Cert/example.pfx"), "123456789");
            }

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddConfigurationStore<IdentityServerConfigurationDbContext>()
                .AddOperationalStore<IdentityServerPersistedGrantDbContext>()
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService>();

            if (env.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                builder.AddSigningCredential(cert);
            }


            return services;
        }


        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IIdentityRepository, IdentityRepository>()
                .AddScoped<IClientRepository, ClientRepository>()
                .AddScoped<IIdentityResourceRepository, IdentityResourceRepository>()
                .AddScoped<IApiResourceRepository, ApiResourceRepository>()
                .AddScoped<IApiScopeRepository, ApiScopeRepository>()
                .AddScoped<IPermissionRepository, PermissionRepository>()
                .AddScoped<IRolePermissionsRepository, RolePermissionsRepository>();
            return services;
        }



        private static IServiceCollection RegisterDbContexts<TApplicationIdentityDbContext, TIdentityServerConfigurationDbContext, TIdentityServerPersistedGrantDbContext>(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        where TApplicationIdentityDbContext : DbContext
        where TIdentityServerConfigurationDbContext : DbContext, IAdminConfigurationDbContext
        where TIdentityServerPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
        {
            if (env.EnvironmentName == "IntegrationTest")
            {
                var dbName = "ApplicationDb";
                services.AddDbContext<ApplicationDbContext>(opt => opt
                    .UseInMemoryDatabase(dbName));

                services.AddConfigurationDbContext<TIdentityServerConfigurationDbContext>(options => options.ConfigureDbContext = b =>
                    b.UseInMemoryDatabase(dbName));

                services.AddOperationalDbContext<TIdentityServerPersistedGrantDbContext>(options => options.ConfigureDbContext = b =>
                    b.UseInMemoryDatabase(dbName));

            }
            else
            {
                var applicationIdentityConnectionString =
                    configuration.GetConnectionString(ConfigurationConstants.ApplicationIdentityConnectionStringKey);
                var adminConfigurationConnectionString =
                    configuration.GetConnectionString(ConfigurationConstants.AdminConfigurationConnectionStringKey);
                var persistedConfigurationConnectionString =
                    configuration.GetConnectionString(ConfigurationConstants.PersistedConfigurationConnectionStringKey);

                services.AddDbContext<TApplicationIdentityDbContext>(options =>
                    options
                        .UseSqlServer(
                            applicationIdentityConnectionString,
                            b => b.MigrationsAssembly(typeof(TApplicationIdentityDbContext).Assembly.FullName))
                );

                services.AddConfigurationDbContext<TIdentityServerConfigurationDbContext>(options => options.ConfigureDbContext = b =>
                    b.UseSqlServer(adminConfigurationConnectionString, sql => sql.MigrationsAssembly(typeof(TApplicationIdentityDbContext).Assembly.FullName)));

                services.AddOperationalDbContext<TIdentityServerPersistedGrantDbContext>(options => options.ConfigureDbContext = b =>
                    b.UseSqlServer(persistedConfigurationConnectionString, sql => sql.MigrationsAssembly(typeof(TIdentityServerPersistedGrantDbContext).Assembly.FullName)));

            }

            services.AddScoped<IAdminConfigurationDbContext, IdentityServerConfigurationDbContext>();
            services.AddScoped<IAdminPersistedGrantDbContext, IdentityServerPersistedGrantDbContext>();

            return services;
        }
    }
}
