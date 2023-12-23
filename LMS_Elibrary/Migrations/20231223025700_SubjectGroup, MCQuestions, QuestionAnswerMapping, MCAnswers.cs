using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Elibrary.Migrations
{
    public partial class SubjectGroupMCQuestionsQuestionAnswerMappingMCAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MCAnswers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MCAnswers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MCQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MCKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isSingleChoice = table.Column<bool>(type: "bit", nullable: false),
                    Creater = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    SubjectGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MCQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MCQuestions_SubjectGroups_SubjectGroupId",
                        column: x => x.SubjectGroupId,
                        principalTable: "SubjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MCQuestions_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswerMapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MCAnswerId = table.Column<int>(type: "int", nullable: false),
                    MCQuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswerMapping", x => x.id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerMapping_MCAnswers_MCAnswerId",
                        column: x => x.MCAnswerId,
                        principalTable: "MCAnswers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerMapping_MCQuestions_MCQuestionId",
                        column: x => x.MCQuestionId,
                        principalTable: "MCQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MCQuestions_SubjectGroupId",
                table: "MCQuestions",
                column: "SubjectGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MCQuestions_SubjectId",
                table: "MCQuestions",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswerMapping_MCAnswerId",
                table: "QuestionAnswerMapping",
                column: "MCAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswerMapping_MCQuestionId",
                table: "QuestionAnswerMapping",
                column: "MCQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionAnswerMapping");

            migrationBuilder.DropTable(
                name: "MCAnswers");

            migrationBuilder.DropTable(
                name: "MCQuestions");

            migrationBuilder.DropTable(
                name: "SubjectGroups");
        }
    }
}
