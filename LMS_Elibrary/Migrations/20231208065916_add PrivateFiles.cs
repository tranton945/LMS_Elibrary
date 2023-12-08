using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Elibrary.Migrations
{
    public partial class addPrivateFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrivateFilesId",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PrivateFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Updator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_PrivateFilesId",
                table: "Files",
                column: "PrivateFilesId",
                unique: true,
                filter: "[PrivateFilesId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_PrivateFiles_PrivateFilesId",
                table: "Files",
                column: "PrivateFilesId",
                principalTable: "PrivateFiles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_PrivateFiles_PrivateFilesId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "PrivateFiles");

            migrationBuilder.DropIndex(
                name: "IX_Files_PrivateFilesId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "PrivateFilesId",
                table: "Files");
        }
    }
}
