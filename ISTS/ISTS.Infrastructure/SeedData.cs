using IdentityModel;
using ISTS.Core.Entities.Identity;
using ISTS.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Infrastructure
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                    var systemAdminRole = roleManager.FindByNameAsync("System Admin").Result;
                    if (systemAdminRole == null)
                    {
                        var systemAdminRoleCreate = new ApplicationRole()
                        {
                            Name = "System Admin",
                            RoleDetails = "This is System admin role which can not be deleted",
                            IsDeletable = false
                        };

                        var roleCreatedResult = roleManager.CreateAsync(systemAdminRoleCreate).Result;
                        if (!roleCreatedResult.Succeeded)
                        {
                            throw new Exception(roleCreatedResult.Errors.First().Description);
                        }
                    }

                    var superAdminRole = roleManager.FindByNameAsync("Super Admin").Result;
                    if (superAdminRole == null)
                    {
                        var superAdminRoleCreate = new ApplicationRole()
                        {
                            Name = "Super Admin",
                            RoleDetails = "This is Super admin role which can not be deleted",
                            IsDeletable = false
                        };

                        var roleCreatedResult = roleManager.CreateAsync(superAdminRoleCreate).Result;
                        if (!roleCreatedResult.Succeeded)
                        {
                            throw new Exception(roleCreatedResult.Errors.First().Description);
                        }
                    }

                    var adminRole = roleManager.FindByNameAsync("Admin").Result;
                    if (adminRole == null)
                    {
                        var adminRoleCreate = new ApplicationRole()
                        {
                            Name = "Admin",
                            RoleDetails = "This is admin role which can not be deleted",
                            IsDeletable = false
                        };

                        var roleCreatedResult = roleManager.CreateAsync(adminRoleCreate).Result;
                        if (!roleCreatedResult.Succeeded)
                        {
                            throw new Exception(roleCreatedResult.Errors.First().Description);
                        }
                    }

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    //var alice = userMgr.FindByNameAsync("alice").Result;
                    //if (alice == null)
                    //{
                    //    alice = new ApplicationUser
                    //    {
                    //        UserName = "alice",
                    //        Email = "AliceSmith@email.com",
                    //        EmailConfirmed = true,
                    //        IsDeletable = false
                    //    };
                    //    var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                    //    if (!result.Succeeded)
                    //    {
                    //        throw new Exception(result.Errors.First().Description);
                    //    }

                    //    result = userMgr.AddClaimsAsync(alice, new Claim[]{
                    //        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    //        new Claim(JwtClaimTypes.GivenName, "Alice"),
                    //        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    //        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    //    }).Result;
                    //    if (!result.Succeeded)
                    //    {
                    //        throw new Exception(result.Errors.First().Description);
                    //    }

                    //    var assignRoleResult = userMgr.AddToRoleAsync(alice, "Admin").Result;
                    //}

                    var alice = userMgr.FindByNameAsync("alice").Result;
                    if (alice == null)
                    {
                        alice = new ApplicationUser
                        {
                            UserName = "alice",
                            Email = "AliceSmith@email.com",
                            EmailConfirmed = true,
                            IsDeletable = false
                        };
                        var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        var assignRoleResult = userMgr.AddToRoleAsync(alice, "Admin").Result;
                    }

                    var bob = userMgr.FindByNameAsync("bob").Result;
                    if (bob == null)
                    {
                        bob = new ApplicationUser
                        {
                            UserName = "bob",
                            Email = "BobSmith@email.com",
                            EmailConfirmed = true,
                            IsDeletable = false
                        };
                        var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        var assignRoleResult = userMgr.AddToRoleAsync(bob, "Super Admin").Result;
                    }


                    //Seed Permission
                    //SeedPermissionToSystemAdminRole();
                }
            }
        }
    }
}
