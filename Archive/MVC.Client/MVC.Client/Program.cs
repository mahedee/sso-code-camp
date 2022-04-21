
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(opt =>{
    opt.DefaultScheme = "Cookies";
    opt.DefaultChallengeScheme = "oidc";
}).AddCookie("Cookies")
.AddOpenIdConnect("oidc", opt =>
{
    opt.SignInScheme = "Cookies";
    opt.Authority = "https://localhost:5005";
    opt.ClientId = "mvc-client";
    opt.ResponseType = "code id_token";
    opt.SaveTokens = true;
    opt.ClientSecret = "MVCSecret";
    opt.GetClaimsFromUserInfoEndpoint = true;

    opt.ClaimActions.DeleteClaim("sid");
    opt.ClaimActions.DeleteClaim("idp");

    opt.Scope.Add("address");
    //opt.ClaimActions.MapUniqueJsonKey("address", "address");

    opt.Scope.Add("roles");
    opt.ClaimActions.MapUniqueJsonKey("role", "role");

    opt.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "role"
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
