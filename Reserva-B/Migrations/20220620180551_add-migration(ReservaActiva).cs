using Microsoft.EntityFrameworkCore.Migrations;

namespace NT1_2022_1C_B_G2.Migrations
{
    public partial class addmigrationReservaActiva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReservaActiva",
                table: "Reservas",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservaActiva",
                table: "Reservas");
        }
    }
}
