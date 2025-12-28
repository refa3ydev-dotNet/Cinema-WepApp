using Core;
using Core.Entities;
using Core.Entities.Relations;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
    public class MoviesDbContext:DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext>options):base(options)
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }

    }
}
