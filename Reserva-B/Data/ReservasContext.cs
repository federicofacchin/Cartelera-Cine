using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NT1_2022_1C_B_G2.Models;
using System;
using System.Threading.Tasks;

namespace NT1_2022_1C_B_G2.Data
{
    public class ReservasContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>
    {
        public ReservasContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");

            //agregar bloques try catch donde corresponda, ver codigo estacionamiento B
            //modelBuilder.Entity<IdentityRole<int>>().HasIndex(r => r.Name).IsUnique(); este tendria que ser con un valor unico, por ejemplo el dni aunque podrian haber dni repetido
            modelBuilder.Entity<Pelicula>().HasIndex(p => p.Titulo).IsUnique();
            modelBuilder.Entity<TipoSala>().HasIndex(ts => ts.Nombre).IsUnique();
            modelBuilder.Entity<Genero>().HasIndex(g => g.Nombre).IsUnique();
            modelBuilder.Entity<Sala>().HasIndex(s => s.Numero).IsUnique();
            modelBuilder.Entity<Funcion>().HasIndex(f => new { f.FechaYHora , f.SalaId , f.PeliculaId }).IsUnique();

        }


        public DbSet<Admin> Admins { get; set; }

        public DbSet<Rol> Roles { get; set; }
        
        public DbSet<Persona> Personas { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Empleado> Empleados { get; set; }

        public DbSet<Funcion> Funciones { get; set; }

        public DbSet<Pelicula> Peliculas { get; set;}

        public DbSet<Reserva> Reservas { get; set; }

        public DbSet<Sala> Salas { get; set; }

        public DbSet<TipoSala> TipoSalas { get; set; }

        public DbSet<Genero> Generos { get; set; }

    }
}
