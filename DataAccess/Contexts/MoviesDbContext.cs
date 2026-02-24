using Core;
using Core.Entities;
using Core.Entities.Relations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class MoviesDbContext : IdentityDbContext<ApplicationUser>
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
        {

        }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<MovieSchedule> MovieSchedules { get; set; }
        public DbSet<ActorMovie> ActorMovies { get; set; }
        public DbSet<CinemaMovie> CinemaMovies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<DirectorMovie> DirectorMovies { get; set; }
        public DbSet<ProducerMovie> ProducerMovies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ActorMovie>().HasKey(am => new
            {
                am.ActorId,
                am.MovieId
            });
            modelBuilder.Entity<ActorMovie>().HasOne(m => m.Movie).WithMany(am => am.ActorMovies).HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<ActorMovie>().HasOne(m => m.Actor).WithMany(am => am.ActorMovies).HasForeignKey(m => m.ActorId);


            modelBuilder.Entity<CinemaMovie>().HasKey(cm => new
            {
                cm.CinemaId,
                cm.MovieId
            });

            modelBuilder.Entity<CinemaMovie>().HasOne(m => m.Movie).WithMany(cm => cm.CinemaMovies).HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<CinemaMovie>().HasOne(m => m.Cinema).WithMany(cm => cm.CinemaMovies).HasForeignKey(m => m.CinemaId);

            modelBuilder.Entity<Movie>()
                .Property(m => m.Price)
                .HasPrecision(10, 2);
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<DirectorMovie>().HasKey(dm=>new {
                dm.DirectorId,
                dm.MovieId
            });
            modelBuilder.Entity<DirectorMovie>()
                .HasOne(m => m.Movie)
                .WithMany(dm => dm.DirectorMovies)
                .HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<DirectorMovie>()
                .HasOne(m => m.Director)
                .WithMany(dm => dm.DirectorMovie)
                .HasForeignKey(m => m.DirectorId);
            modelBuilder.Entity<ProducerMovie>().HasKey(dm=>new {
                dm.ProducerId,
                dm.MovieId
            });
            modelBuilder.Entity<ProducerMovie>()
                .HasOne(m => m.Movie)
                .WithMany(dm => dm.ProducerMovies)
                .HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<ProducerMovie>()
                .HasOne(m => m.Producer)
                .WithMany(dm => dm.ProducerMovies)
                .HasForeignKey(m => m.ProducerId);
        }

    }
}
