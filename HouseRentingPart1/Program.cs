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