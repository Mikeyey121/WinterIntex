using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WinterIntex.Data;
using WinterIntex.Models;
namespace WinterIntex {
    public class Program
    {
        public static async Task Main(string[] args)
        {

            // Create builder variable
            var builder = WebApplication.CreateBuilder(args);

            // Retrieve the password from user secrets
            var dbPassword = builder.Configuration["DbPassword"]
                ?? throw new InvalidOperationException("DbPassword not found in user secrets.");

            // Retrieve the UserID from user secrets
            var dbUserId = builder.Configuration["DbUserId"]
                ?? throw new InvalidOperationException("DbUserId not found in user secrets.");

            // Construct the connection string
            var conStrBuilder = new SqlConnectionStringBuilder(
                builder.Configuration.GetConnectionString("DefaultConnection"));
            conStrBuilder.UserID = dbUserId;
            conStrBuilder.Password = dbPassword;
            var connectionString = conStrBuilder.ConnectionString;

            // Adding configuration for Google login
            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });

            // Adding the configuration for the repository pattern
            builder.Services.AddScoped<IProductRepository, EFProductRepository>();
            builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();


            // Connection for sql server database 
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging());
            builder.Services.AddDbContext<WinterIntexContext>(options =>
                options.UseSqlServer(connectionString)
                        .EnableSensitiveDataLogging());

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
                .AddRoles<IdentityRole>()
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

            //Blazor Services
            builder.Services.AddServerSideBlazor();

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

            // Configuring for authentication
            builder.Services.AddAuthentication();

            // Add the connection to the IHttpContextAccessor so we can use the current session for each user
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Setting app to builder.Build
            var app = builder.Build();

            // Configure for authorization
            app.UseAuthorization();
            app.UseAuthentication();


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
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; connect-src 'self' http://localhost:* ws://localhost:*; " +
                    "img-src 'self' https://m.media-amazon.com/images/ https://www.lego.com/cdn/ https://images.brickset.com/sets/ https://www.brickeconomy.com/resources/images/sets/ data:; " +
                    "script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; font-src 'self' https://fonts.gstatic.com");

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


            // Configuration for mapping controller routes

            // Configure for razor pages
            app.MapRazorPages();

            // pageSizes=5&pageNum=1&color=&categoryDescription=
            app.MapControllerRoute("productFilters", "Products/{pageSizes}/{pageNum}/{color?}/{categoryDescription?}", new { Controller = "Home", Action = "Index" });


            app.MapControllerRoute("pagenumandcolor", "{color}/Page{pageNum}", new { Controller = "Home", Action = "Index" });
            app.MapControllerRoute("pagenumandcategory", "{categoryDescription}/Page{pageNum}", new { Controller = "Home", Action = "Index" });

            app.MapControllerRoute("page", "Page/{pageNum}", new { Controller = "Home", Action = "Index", pageNum = 1 });
            app.MapControllerRoute("color", "{color}", new { Controller = "Home", Action = "Index", pageNum = 1 });
            app.MapControllerRoute("productCategory", "{categoryDescription}", new { Controller = "Home", Action = "Index", pageNum = 1 });
            app.MapControllerRoute("pagination", "Products/Page{pageNum}", new { Controller = "Home", Action = "Index", pageNum = 1 });

            app.MapDefaultControllerRoute();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager =
                    scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Member" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager =
                    scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string email = "admin@admin.com";
                string password = "Test1234!@#$aaaaaaaaaa";
                
                if(await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = email;
                    user.Email = email;
                    user.EmailConfirmed = true;

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // Run that bad boy
            app.Run();
        } } }
