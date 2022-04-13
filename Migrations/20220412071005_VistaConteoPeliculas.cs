using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCorePeliculas.Migrations
{
    public partial class VistaConteoPeliculas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW [dbo].[PeliculasConConteos]
                AS
                Select Id, Titulo,
                (Select count(*) from GeneroPelicula Where PeliculasId = Peliculas.Id) as CantidadGeneros,
                (Select count(distinct CineId) FROM PeliculaSalaDeCine	INNER JOIN SalasDeCine
                ON SalasDeCine.Id = PeliculaSalaDeCine.SalasDeCineId
                WHERE PeliculasId = Peliculas.Id) as CantidadCines,
                (Select count(*) from PeliculasActores Where PeliculaId = Peliculas.Id) as CantidadActores
                FROM Peliculas
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //se ejecuta cuando removemos una migración a través de la línea de comandos
            migrationBuilder.Sql("DROP VIEW [dbo].[PeliculasConConteos]"); //Borramos la vista
        }
    }
}
