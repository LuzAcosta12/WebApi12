using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi29.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación uno a muchos entre Rol y Usuario
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Roles)
                .WithMany()
                .HasForeignKey(u => u.FkRol);

            // Insertar en la tabla Roles
            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    PkRol = 1,
                    Nombre = "Admin"
                },
                new Rol
                {
                    PkRol = 2,
                    Nombre = "Usuario"
                });

            // Insertar en tabla Usuario
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    PkUsuario = 1,
                    Nombre = "luz",
                    UserName = "Admin",    // <--- nombre de usuario real
                    Password = "123",      // <--- contraseña simple para pruebas
                    FkRol = 1              // <--- tiene el rol de Admin
                },
                new Usuario
                {
                    PkUsuario = 2,
                    Nombre = "acosta",
                    UserName = "Usuario",
                    Password = "123",
                    FkRol = 2
                });
        }
    }
}
