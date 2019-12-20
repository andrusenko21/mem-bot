using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemBotDataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Command = table.Column<string>(nullable: false),
                    Content = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mem", x => x.Id);
                    table.UniqueConstraint("AK_Mem_Command", x => x.Command);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mem");
        }
    }
}
