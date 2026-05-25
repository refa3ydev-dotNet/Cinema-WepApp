using Business.Managers.Accounts;
using Business.Managers.Actors;
using Business.Managers.Admin;
using Business.Managers.Agent;
using Business.Managers.Bookings;
using Business.Managers.Categories;
using Business.Managers.Cinemas;
using Business.Managers.Directors;
using Business.Managers.Favorites;
using Business.Managers.Movies;
using Business.Managers.Producers;
using Business.Managers.Rooms;
using Business.Managers.Schedule;
using Business.Managers.Users;
using Business.Services.TmdbService;
using Core.Entities;
using DataAccess.Contexts;
using DataAccess.Repositories;
using DataAccess.Repositories.ACTOR;
using DataAccess.Repositories.Admin;
using DataAccess.Repositories.CATEGORY;
using DataAccess.Repositories.CINEMA;
using DataAccess.Repositories.Dashboard;
using DataAccess.Repositories.DIRECTOR;
using DataAccess.Repositories.Favorite;
using DataAccess.Repositories.MOVIE;
using DataAccess.Repositories.PRODUCER;
using DataAccess.Repositories.ROOM;
using DataAccess.Repositories.Schedule;
using DataAccess.Repositories.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movies_web_app.Helper;
using Movies_web_app.Services;

namespace Movies_web_app
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
//Repositories
builder.Services.AddScoped<IActorRepository,ActorRepository>();
builder.Services.AddScoped<IRoomRepository,RoomRepository>();
builder.Services.AddScoped<IProducerRepository, ProducerRepository>();
builder.Services.AddScoped<ICinemaRepository, CinemaRepository>();
builder.Services.AddScoped<IMovieRepository,MovieRepository>();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IDirectorRepository,DirectorRepository>();
builder.Services.AddScoped<IAgentDashboardRepository, AgentDashboardRepository>();
builder.Services.AddScoped<IMovieScheduleRepository, MovieScheduleRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IAdminDashboardRepository,AdminDashboardRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IActorsManager,ActorsManager>();
builder.Services.AddScoped<IRoomManager, RoomManager>();
builder.Services.AddScoped<IProducersManager, ProducersManager>();
builder.Services.AddScoped<ICinemasManager, CinemasManager>();
builder.Services.AddScoped<IMovieManager,MovieManager>();
builder.Services.AddScoped<ICategoryManager,CategoryManager>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IDirectorManager,DirectorManager>();
builder.Services.AddScoped<IAccountManager, AccountManager>();
builder.Services.AddScoped<IAgentDashboardManager, AgentDashboardManager>();
builder.Services.AddHttpClient<ITmdbService, TmdbService>();
builder.Services.AddScoped<IMovieScheduleManager, MovieScheduleManager>();
builder.Services.AddScoped<IBookingManager, BookingManager>();
builder.Services.AddScoped<IFavoriteManager, FavoriteManager>();
builder.Services.AddScoped<IAdminManager, AdminManager>();
builder.Services.AddScoped<IUserManager, UserManager>();

            // DbContext configuration using the connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MoviesDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Strengthened password policy for production security
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true; // Require special character
                options.Password.RequiredLength = 10; // Increased from 8 to 10
                options.Password.RequiredUniqueChars = 3;
            })
            .AddEntityFrameworkStores<MoviesDbContext>()
            .AddDefaultTokenProviders()
            .AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>();

            // Account lockout configuration
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Security headers middleware
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdn.jsdelivr.net https://cdn.tailwindcss.com; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.jsdelivr.net; font-src 'self' https://fonts.gstatic.com https://cdn.jsdelivr.net; img-src 'self' data: https:; frame-src 'self' https:;");
    
    // Add cache headers for static assets
    if (context.Request.Path.Value?.Contains("/wwwroot/") == true || 
        context.Request.Path.Value?.StartsWith("/css/") == true ||
        context.Request.Path.Value?.StartsWith("/js/") == true)
    {
        context.Response.Headers.Append("Cache-Control", "public, max-age=31536000, immutable");
    }
    
    await next();
});

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await AppDbInitializer.TaskSeedRoleAsync(app);
await AppDbInitializer.TaskSeedAdminUserAsync(app);
app.Run();
        }
    }
}
