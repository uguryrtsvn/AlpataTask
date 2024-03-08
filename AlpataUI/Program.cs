using AlpataBLL;
using AlpataUI.Helpers.ClientHelper;
using AlpataUI.Helpers.FileUploadHelper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using NToastNotify;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 3000,
});
builder.Services.AddResponseCompression();
// Add services to the container.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "Auth";
    options.LoginPath = "/Home/Index";
    options.LogoutPath = new PathString("/Account/SignOut");
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(365);
    options.SlidingExpiration = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Events = new CookieAuthenticationEvents()
    {
        OnValidatePrincipal = async (context) =>
        {
            await Task.Run(() =>
            {
                var now = DateTimeOffset.UtcNow;
                var timeElapsed = now.Subtract(context.Properties.IssuedUtc!.Value);
                var timeRemaining = context.Properties.ExpiresUtc!.Value.Subtract(now);

                if (timeElapsed > TimeSpan.FromMinutes(15) && timeRemaining > TimeSpan.FromMinutes(15))
                {
                    context.Properties.IssuedUtc = DateTimeOffset.UtcNow;
                    context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.Add(options.ExpireTimeSpan);
                    context.ShouldRenew = true;
                }
            });
        }
    };
});
#region HttpClient
builder.Services
    .AddHttpClient<IAlpataClient, AlpataClient>()
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
#endregion

builder.Services.AddScoped<IFileUploadService, FileUploadService>();

#region Fluent Validation
builder.Services.AddValidatorsFromAssemblyContaining<IFluentValidator>().AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
#endregion

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//ResponseCompress
app.UseResponseCompression();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseNToastNotify(); 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
