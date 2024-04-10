using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WinterIntex.Data;
using WinterIntex.Models;

// Create builder variable
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Adding configuration for Google login
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();


// Connection for sql server database 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<WinterIntexContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add identity services
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Requirements for secure password
    options.Password.RequiredLength = 20; // At least 20 characters
    options.Password.RequireUppercase = true; // At least 1 uppercase letter
    options.Password.RequireLowercase = true; // At least 1 lowercase letter
    options.Password.RequireDigit = true; // At least 1 digit
    options.Password.RequireNonAlphanumeric = true; // At least 1 special character
    options.Password.RequiredUniqueChars = 6; // At least 6 unique characters
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Configure HSTS options
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
    // options.ExcludedHosts.Add("example.com"); // Use this to exclude specific hosts from HSTS
});


builder.Services.AddRazorPages();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Configuration for client size razor pages

    builder.Services.AddControllersWithViews()
        .AddRazorRuntimeCompilation();



// Memory Cache and Session configuration for session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Configuration for server side blazor apps
builder.Services.AddServerSideBlazor();

// Add the scope for the cart
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));


// Add the connection to the IHttpContextAccessor so we can use the current session for each user
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Setting app to builder.Build
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure so that http request come in as https
app.UseHttpsRedirection();

// Configure for static files we want to use
app.UseStaticFiles();

// Add CSP header middleware
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; connect-src 'self' http://localhost:* ws://localhost:*; img-src 'self' data:; " +
        "script-src 'self'; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; font-src 'self' https://fonts.gstatic.com");
    await next();
});

app.UseSession();

// Configure for routing
app.UseRouting();

// Configure for HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// Configure for authorization
app.UseAuthorization();

// Configuration for mapping controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Configure for razor pages
app.MapRazorPages();

app.MapControllerRoute("pagenumandcolor", "{color}/Page{pageNum}", new { Controller = "Home", Action = "Index" });
app.MapControllerRoute("pagenumandcategory", "{categoryDescription}/Page{pageNum}", new { Controller = "Home", Action = "Index" });

app.MapControllerRoute("page", "Page/{pageNum}", new { Controller = "Home", Action = "Index", pageNum = 1 });
app.MapControllerRoute("color", "{color}", new { Controller = "Home", Action = "Index", pageNum = 1 });
app.MapControllerRoute("productCategory", "{categoryDescription}", new { Controller = "Home", Action = "Index", pageNum = 1 });
app.MapControllerRoute("pagination", "Products/Page{pageNum}", new { Controller = "Home", Action = "Index", pageNum = 1 });



// Run that bad boy
app.Run();
