using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Elibrary.Migrations
{
    public partial class fixquestionandanswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Contain",
                table: "Questions",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "Contain",
                table: "Answers",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Questions",
                newName: "Contain");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Answers",
                newName: "Contain");
        }
    }
}
