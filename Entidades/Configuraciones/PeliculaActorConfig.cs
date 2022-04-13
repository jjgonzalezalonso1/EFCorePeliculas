using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCorePeliculas.Entidades.Configuraciones
{
    public class PeliculaActorConfig : IEntityTypeConfiguration<PeliculaActor>
    {
        public void Configure(EntityTypeBuilder<PeliculaActor> builder)
        { //agregamos la clave compuesta por dos campos
            builder.HasKey(prop =>
             new { prop.PeliculaId, prop.ActorId });

            builder.Property(prop => prop.Personaje)
                .HasMaxLength(150);
        }
    }
}
