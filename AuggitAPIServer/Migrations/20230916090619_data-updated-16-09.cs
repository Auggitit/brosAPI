using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuggitAPIServer.Migrations
{
    public partial class dataupdated1609 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "remarks",
                table: "voucherEntry",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "remarks",
                table: "overdueentry",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "remarks",
                table: "accountentry",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
              name: "remarks",
              table: "voucherEntry");

            migrationBuilder.DropColumn(
                name: "remarks",
                table: "overdueentry");

            migrationBuilder.DropColumn(
                name: "remarks",
                table: "accountentry");
        }
    }
}
