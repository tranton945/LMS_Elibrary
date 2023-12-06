using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Elibrary.Migrations
{
    public partial class addApproveDatefordocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_DocumentId",
                table: "Files");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                table: "Documents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_DocumentId",
                table: "Files",
                column: "DocumentId",
                unique: true,
                filter: "[DocumentId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_DocumentId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ApproveDate",
                table: "Documents");

            migrationBuilder.CreateIndex(
                name: "IX_Files_DocumentId",
                table: "Files",
                column: "DocumentId");
        }
    }
}
