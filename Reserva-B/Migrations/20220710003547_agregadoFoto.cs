using Microsoft.EntityFrameworkCore.Migrations;

namespace NT1_2022_1C_B_G2.Migrations
{
    public partial class agregadoFoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Peliculas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Peliculas");
        }
    }
}
