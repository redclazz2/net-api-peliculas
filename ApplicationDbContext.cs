using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using net_api_peliculas.Entidades;

namespace net_api_peliculas
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options){}
        public DbSet<Genero> Generos { get; set; }

    }
}