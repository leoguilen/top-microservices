using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TOP.ProfileService.Infra.Data.Migrations
{
    public partial class added_lastSync_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LastSynchronization",
                columns: table => new
                {
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LastSynchronization");
        }
    }
}
