using OAuth.STS.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add identity server to the application
builder.Services.AddIdentityServer()
    .AddInMemoryApiScopes(InMemoryConfig.GetApiScopes())
    .AddInMemoryApiResources(InMemoryConfig.GetApiResources())
    .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
    .AddTestUsers(InMemoryConfig.GetUsers())
    .AddInMemoryClients(InMemoryConfig.GetClients())
    .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

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

// Add identity server to request pipeline
app.UseIdentityServer();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
