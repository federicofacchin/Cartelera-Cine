using Microsoft.EntityFrameworkCore.Migrations;

namespace NT1_2022_1C_B_G2.Migrations
{
    public partial class butacasDisponibles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ButacasDisponibles",
                table: "Funciones");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ButacasDisponibles",
                table: "Funciones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
