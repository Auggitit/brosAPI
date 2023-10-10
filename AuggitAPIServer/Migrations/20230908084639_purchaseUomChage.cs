using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuggitAPIServer.Migrations
{
    public partial class purchaseUomChage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "uom",
                table: "vSGrnDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uomcode",
                table: "vSGrnDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uom",
                table: "vGrnDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uomcode",
                table: "vGrnDetails",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uom",
                table: "vSGrnDetails");

            migrationBuilder.DropColumn(
                name: "uomcode",
                table: "vSGrnDetails");

            migrationBuilder.DropColumn(
                name: "uom",
                table: "vGrnDetails");

            migrationBuilder.DropColumn(
                name: "uomcode",
                table: "vGrnDetails");
        }
    }
}
