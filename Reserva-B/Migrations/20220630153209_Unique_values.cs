using Microsoft.EntityFrameworkCore.Migrations;

namespace NT1_2022_1C_B_G2.Migrations
{
    public partial class Unique_values : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TipoSalas_Nombre",
                table: "TipoSalas",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salas_Numero",
                table: "Salas",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Peliculas_Titulo",
                table: "Peliculas",
                column: "Titulo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Generos_Nombre",
                table: "Generos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funciones_FechaYHora_SalaId_PeliculaId",
                table: "Funciones",
                columns: new[] { "FechaYHora", "SalaId", "PeliculaId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TipoSalas_Nombre",
                table: "TipoSalas");

            migrationBuilder.DropIndex(
                name: "IX_Salas_Numero",
                table: "Salas");

            migrationBuilder.DropIndex(
                name: "IX_Peliculas_Titulo",
                table: "Peliculas");

            migrationBuilder.DropIndex(
                name: "IX_Generos_Nombre",
                table: "Generos");

            migrationBuilder.DropIndex(
                name: "IX_Funciones_FechaYHora_SalaId_PeliculaId",
                table: "Funciones");
        }
    }
}
