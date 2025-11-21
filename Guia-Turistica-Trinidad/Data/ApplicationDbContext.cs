using guia_turistico.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guia_Turistica_Trinidad.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets en plural (coincide con nombres de entidades)
        public DbSet<Tipos> Tipos { get; set; }
        public DbSet<SitiosTuristicos> SitiosTuristicos { get; set; }
        public DbSet<ImagenesSitios> ImagenesSitios { get; set; }
        public DbSet<Comentarios> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuraciones adicionales si las necesitas
            // Por ejemplo, relaciones, índices, etc.
        }
    }
}
