using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Elibrary.Migrations
{
    public partial class addBlockStudentsinLecture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectOtherInformation_Subjects_SubjectId",
                table: "SubjectOtherInformation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectOtherInformation",
                table: "SubjectOtherInformation");

            migrationBuilder.RenameTable(
                name: "SubjectOtherInformation",
                newName: "SubjectOtherInformations");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectOtherInformation_SubjectId",
                table: "SubjectOtherInformations",
                newName: "IX_SubjectOtherInformations_SubjectId");

            migrationBuilder.AddColumn<bool>(
                name: "BlockStudents",
                table: "Lectures",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectOtherInformations",
                table: "SubjectOtherInformations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectOtherInformations_Subjects_SubjectId",
                table: "SubjectOtherInformations",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectOtherInformations_Subjects_SubjectId",
                table: "SubjectOtherInformations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectOtherInformations",
                table: "SubjectOtherInformations");

            migrationBuilder.DropColumn(
                name: "BlockStudents",
                table: "Lectures");

            migrationBuilder.RenameTable(
                name: "SubjectOtherInformations",
                newName: "SubjectOtherInformation");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectOtherInformations_SubjectId",
                table: "SubjectOtherInformation",
                newName: "IX_SubjectOtherInformation_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectOtherInformation",
                table: "SubjectOtherInformation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectOtherInformation_Subjects_SubjectId",
                table: "SubjectOtherInformation",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
