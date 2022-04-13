using Microsoft.EntityFrameworkCore; //añadido
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCorePeliculas.Entidades.Configuraciones
{
    // implementamos la interface IEntityTypeConfiguration
    // Ctrl + punto.
    // Ctrl + punto.
    public class GeneroConfig : IEntityTypeConfiguration<Genero>
    {
        public void Configure(EntityTypeBuilder<Genero> builder)
        {
            builder.HasKey(prop => prop.Identificador);
            //el campo Identificador es una clave primaria
            builder.Property(prop => prop.Nombre)
                .HasMaxLength(150)
                .IsRequired();
            builder.HasQueryFilter(g => !g.EstaBorrado);
            builder.HasIndex(g => g.Nombre).IsUnique().HasFilter("EstaBorrado='false'");

            builder.Property<DateTime>("FechaCreacion").HasDefaultValueSql("GetDate()").HasColumnType("datetime2");
            //nombre del campo FechaCreacion, valor por defect la fecha de hoy
        }
    }
}
