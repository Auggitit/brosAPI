using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuggitAPIServer.Migrations
{
    public partial class hsncatchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "catunder",
                table: "mCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "catunder",
                table: "mCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
