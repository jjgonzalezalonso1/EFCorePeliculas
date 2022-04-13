using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCorePeliculas.Entidades.Configuraciones
{
    public class CineConfig : IEntityTypeConfiguration<Cine>
    {
        public void Configure(EntityTypeBuilder<Cine> builder)
        {
            builder.Property(prop => prop.Nombre)
              .HasMaxLength(150)
              .IsRequired();

            builder.HasOne(c => c.CineOferta)
                //indicamos la propiedad de navegacion que va desde cine a cineoferta
                .WithOne() // el CineOferta tiene solo un cine
                .HasForeignKey<CineOferta>(co => co.CineId);
            //configuro la clave foranea en CineOferta

            builder.HasMany(c => c.SalasDeCine)
                //un Cine tiene muchas SalasDeCine
               .WithOne(s => s.Cine) //una SalasDeCine tiene un Cine
               .HasForeignKey(s => s.CineId);
            //clave foranea en esta relación
        }
    }
}
