using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Elibrary.Migrations
{
    public partial class ClassRoomNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassRoomNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRoomNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassRoomNotificationLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassRoomId = table.Column<int>(type: "int", nullable: false),
                    ClassRoomNotificationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRoomNotificationLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassRoomNotificationLinks_ClassRoomNotifications_ClassRoomNotificationId",
                        column: x => x.ClassRoomNotificationId,
                        principalTable: "ClassRoomNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassRoomNotificationLinks_ClassRooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassRoomNotificationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedUsers_ClassRoomNotifications_ClassRoomNotificationId",
                        column: x => x.ClassRoomNotificationId,
                        principalTable: "ClassRoomNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassRoomNotificationLinks_ClassRoomId",
                table: "ClassRoomNotificationLinks",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRoomNotificationLinks_ClassRoomNotificationId",
                table: "ClassRoomNotificationLinks",
                column: "ClassRoomNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedUsers_ClassRoomNotificationId",
                table: "SelectedUsers",
                column: "ClassRoomNotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassRoomNotificationLinks");

            migrationBuilder.DropTable(
                name: "SelectedUsers");

            migrationBuilder.DropTable(
                name: "ClassRoomNotifications");
        }
    }
}
