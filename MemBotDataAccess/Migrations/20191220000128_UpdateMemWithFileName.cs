using Microsoft.EntityFrameworkCore.Migrations;

namespace MemBotDataAccess.Migrations
{
    public partial class UpdateMemWithFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Mem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Mem");
        }
    }
}
