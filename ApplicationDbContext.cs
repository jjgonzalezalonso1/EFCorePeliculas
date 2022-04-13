using EFCorePeliculas.Entidades;
using EFCorePeliculas.Entidades.Configuraciones; //agregado
using EFCorePeliculas.Entidades.Seeding;
using EFCorePeliculas.Entidades.SinLlaves;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EFCorePeliculas
{
    //hereda de DbContext, pulso dos veces Ctrl + punto
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { //utilizamos el sistema de inyección de dependencias
         }
        //escribo prop tab-tab
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
            //Siempre que agregemos un campo DateTime por defecto se mapea a date
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Ctrl + pto
            // modelBuilder.ApplyConfiguration(new GeneroConfig());  //para uno concreto
            //para que coja todas las configuraciones
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            SeedingModuloConsulta.Seed(modelBuilder);
            SeedingPersonaMensaje.Seed(modelBuilder);   

            modelBuilder.Entity<CineSinUbicacion>()
                .HasNoKey().ToSqlQuery("Select Id, Nombre FROM Cines").ToView(null);
            //.ToView(null) para que no se agrege una tabla en la base de datos con el esquema de CineSinUbicacion


            modelBuilder.Entity<PeliculaConConteos>().HasNoKey().ToView("PeliculasConConteos");
            // nombre de la vista CREATE VIEW [dbo].[PeliculasConConteos]


            foreach (var tipoEntidad in modelBuilder.Model.GetEntityTypes())
            { //iteramos todas las entidades de nuestra aplicacion
                foreach (var propiedad in tipoEntidad.GetProperties())
                {//iteramos todas las propiedades de todas las entidades
                    if (propiedad.ClrType == typeof(string) && propiedad.Name.Contains("URL", StringComparison.CurrentCultureIgnoreCase))
                    {
                        // si es un string y la propiedad contiene URL , no importa si es en minusculas o en mayúsculas
                        propiedad.SetIsUnicode(false);
                        propiedad.SetMaxLength(500);
                    }
                }
            }



            // modelBuilder.Entity<Log>().Property(l => l.Id).ValueGeneratedNever();   
            // modelBuilder.Ignore<Direccion>();
            //modelBuilder.Entity<Cine>().Property(prop => prop.Precio)
            //    .HasPrecision(precision: 9, scale: 2);      

        }

        public DbSet<Genero>Generos { get; set; }
        public DbSet<Actor> Actores { get; set; } //nombre de la tabla
        public DbSet<Cine> Cines { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<CineOferta> CinesOfertas { get; set; }
        public DbSet<SalaDeCine> SalasDeCine { get; set; }
        public DbSet<PeliculaActor> PeliculasActores { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<CineSinUbicacion> CinesSinUbicacion { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
    }
}
