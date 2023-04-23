using AuthorizeOnRoute.Requirements;
using AuthorizeOnRoute.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<BlazorSchoolUserService>();
builder.Services.AddScoped<BlazorSchoolAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<BlazorSchoolAuthenticationStateProvider>());
builder.Services.AddScoped<IAuthorizationHandler, AdultRequirementHandler>();
builder.Services.AddAuthorizationCore(config =>
{
    config.AddPolicy("AdultOnly", policy => policy.AddRequirements(new AdultRequirement()));

    config.AddPolicy("AdultAdminOnly", policy =>
    {
        policy.AddRequirements(new AdultRequirement());
        policy.RequireRole("admin");
    });
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
