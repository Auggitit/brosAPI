using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuggitAPIServer.Migrations
{
    public partial class pochange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "qtymt",
                table: "vSPODetails");

            migrationBuilder.DropColumn(
                name: "qtymt",
                table: "vPODetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "qtymt",
                table: "vSPODetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "qtymt",
                table: "vPODetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
