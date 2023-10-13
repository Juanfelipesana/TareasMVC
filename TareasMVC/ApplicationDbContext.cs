using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TareasMVC.Entidades;

namespace TareasMVC
{
    //Es la pieza central relacionada con EF Core, aquí podemos poner las configuraciones que queramos tener en nuestras tablas
    public class ApplicationDbContext : IdentityDbContext 
    {
        //Aquí podemos configurar lo básico, connection strings, etc.
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Configuramos usando el Api.
            //modelBuilder.Entity<Tarea>().Property(t=> t.Titulo).HasMaxLength(250).IsRequired();
        }
        //Indicamos con DBSet que queremos crear una tabla a partir de la clase tareas, y la tabla se llama Tareas. Esto se realizará con migraciones
        //La migración la realizamos en el Package Manager Console, la línea es "Add-Migration (Nombre de la tabla)"
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Paso> Pasos { get; set; }
        public DbSet<ArchivoAdjunto> ArchivosAdjuntos { get; set; }
    }
}
