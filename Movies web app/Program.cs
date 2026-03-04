using Business.Managers.Directors;
using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
            builder.Services.AddScoped<Business.Managers.Actors.IActorsManager,
                Business.Managers.Actors.ActorsManager>();
            builder.Services.AddScoped<DataAccess.Repositories.ACTOR.IActorRepository,
                DataAccess.Repositories.ACTOR.ActorRepository>();

            builder.Services.AddScoped<Business.Managers.Rooms.IRoomManager,
                Business.Managers.Rooms.RoomManager>();
            builder.Services.AddScoped<DataAccess.Repositories.ROOM.IRoomRepository,
                DataAccess.Repositories.ROOM.RoomRepository>();

            builder.Services.AddScoped<Business.Managers.Producers.IProducersManager,
                Business.Managers.Producers.ProducersManager>();
            builder.Services.AddScoped<DataAccess.Repositories.PRODUCER.IProducerRepository,
                DataAccess.Repositories.PRODUCER.ProducerRepository>();

            builder.Services.AddScoped<Business.Managers.Cinemas.ICinemasManager,
                Business.Managers.Cinemas.CinemasManager>();
            builder.Services.AddScoped<DataAccess.Repositories.CINEMA.ICinemaRepository,
                DataAccess.Repositories.CINEMA.CinemaRepository>();

            builder.Services.AddScoped<Business.Managers.Movies.IMovieManager,
                Business.Managers.Movies.MovieManager>();
            builder.Services.AddScoped<DataAccess.Repositories.MOVIE.IMovieRepository,
                DataAccess.Repositories.MOVIE.MovieRepository>();

            builder.Services.AddScoped<Business.Managers.Categories.ICategoryManager,
                Business.Managers.Categories.CategoryManager>();
            builder.Services.AddScoped<DataAccess.Repositories.CATEGORY.ICategoryRepository,
                DataAccess.Repositories.CATEGORY.CategoryRepository>();
            builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddScoped<Business.Managers.Directors.IDirectorManager,
                Business.Managers.Directors.DirectorManager>();
            builder.Services.AddScoped<DataAccess.Repositories.DIRECTOR.IDirectorRepository,
                DataAccess.Repositories.DIRECTOR.DirectorRepository>();
            builder.Services.AddScoped<Business.Managers.Accounts.IAccountManager, Business.Managers.Accounts.AccountManager>();

            // ? DbContext configuration using the connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MoviesDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity < ApplicationUser, IdentityRole>(Options =>
            {
                Options.Password.RequireDigit = true;
                Options.Password.RequireLowercase = true;
                Options.Password.RequireUppercase = true;
                Options.Password.RequireNonAlphanumeric = false;
                Options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<MoviesDbContext>()
            .AddDefaultTokenProviders()
            .AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await AppDbInitializer.TaskSeedRoleAsync(app);
            app.Run();
        }
    }
}
