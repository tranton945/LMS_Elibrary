using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Elibrary.Migrations
{
    public partial class addLikeQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_ClassRooms_ClassRoomId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "ClassRoomId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "LikeQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikeQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikeQuestions_QuestionId",
                table: "LikeQuestions",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_ClassRooms_ClassRoomId",
                table: "Questions",
                column: "ClassRoomId",
                principalTable: "ClassRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_ClassRooms_ClassRoomId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "LikeQuestions");

            migrationBuilder.AlterColumn<int>(
                name: "ClassRoomId",
                table: "Questions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_ClassRooms_ClassRoomId",
                table: "Questions",
                column: "ClassRoomId",
                principalTable: "ClassRooms",
                principalColumn: "Id");
        }
    }
}
