using HouseFlowPart1.Interfaces;
using HouseFlowPart1.Middleware;
using HouseFlowPart1.Models;
using HouseFlowPart1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();


        // to handle 404 or not found routes
        builder.Services.AddMvc().AddRazorPagesOptions(options =>
        {
            options.Conventions.AddPageRoute("/Home/NotFound", "/{*url}");
        });

        var mongoConnectionString = builder.Configuration["MongoDBSettings:ConnectionString"];
        var databaseName = builder.Configuration["MongoDBSettings:DatabaseName"];

        builder.Services.AddSingleton<MongoDBContext>();



        builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
        builder.Services.AddTransient<IHouseTypesService, HouseTypesService>();

        builder.Services.AddScoped<IHouseService, HouseService>();
        builder.Services.AddScoped<ILogService, LogService>();
        builder.Services.AddScoped<IHouseImageService, HouseImageService>();
        builder.Services.AddScoped<IRentedHauseService, RentedHauseService>();


        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x => x.LoginPath = "/Login");

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        // Add the authentication 
        app.UseAuthentication();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();


        // seed db ( define first static datas for data base ) 
        // seed db ( define first static datas for data base ) 
        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;

            // Retrieve the HouseTypesService instance
            var houseTypesService = serviceProvider.GetRequiredService<IHouseTypesService>();
            var houseService = serviceProvider.GetRequiredService<IHouseService>();
            var houseImages = serviceProvider.GetRequiredService<IHouseImageService>();
            var users = serviceProvider.GetRequiredService<IAuthenticationService>();

            // Call the Seed HouseTypes Data method to seed the database
            houseTypesService.SeedHouseTypes();

            // Call the Seed Users Data method to seed the database
            users.SeedData();

            // Call the Seed House Data method to seed the database
            houseService.SeedData();

            // Call the Seed House Images Data method to seed the database
            houseImages.SeedData();
        }

        // add middleware
        app.UseMiddleware<AuthenticationMiddleware>();
        app.UseMiddleware<LogProfilerMiddleware>();

        app.UseStatusCodePagesWithReExecute("/Home/NotFound");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}