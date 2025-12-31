using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Movies_web_app.Services;

namespace Movies_web_app
{
    public class Program
    {
        public static void Main(string[] args)
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

            // ? DbContext configuration using the connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MoviesDbContext>(options =>
                options.UseSqlServer(connectionString));

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
