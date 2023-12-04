using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Elibrary.Migrations
{
    public partial class updatesubjectTopicLectureDocumentFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Lectures_LectureID",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Documents_DocumentId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Subjects");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Descriptions",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Lectures",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "Files",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LectureID",
                table: "Documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_TopicId",
                table: "Lectures",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Lectures_LectureID",
                table: "Documents",
                column: "LectureID",
                principalTable: "Lectures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Documents_DocumentId",
                table: "Files",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Topics_TopicId",
                table: "Lectures",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Lectures_LectureID",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Documents_DocumentId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Topics_TopicId",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_TopicId",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Lectures");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Subjects",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Title",
                table: "Lectures",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Descriptions",
                table: "Lectures",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LectureID",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Lectures_LectureID",
                table: "Documents",
                column: "LectureID",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Documents_DocumentId",
                table: "Files",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
