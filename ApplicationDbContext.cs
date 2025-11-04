using Microsoft.EntityFrameworkCore;
using net_api_peliculas.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace net_api_peliculas
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PeliculasGeneros>()
            .HasKey(e => new { e.GeneroId, e.PeliculaId });

            modelBuilder.Entity<PeliculasCines>()
            .HasKey(e => new { e.CineId, e.PeliculaId });

            modelBuilder.Entity<PeliculasActores>()
            .HasKey(e => new { e.ActorId, e.PeliculaId});
        }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }

        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasCines> PeliculasCines { get; set; }
        
        public DbSet<Rating> RatingsPeliculas { get; set; }
    }
}