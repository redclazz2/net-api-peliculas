using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using net_api_peliculas.Entidades;

namespace net_api_peliculas
{
    public class ApplicationDbContext : DbContext
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
        public DbSet<PeliculasCines> PeliculasCines{ get; set; }
    }
}