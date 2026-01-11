using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNoticeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PostedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PostedByUserName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PostedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notices_Users_PostedByUserId",
                        column: x => x.PostedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NoticeReplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoticeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReplyMessage = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    RepliedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RepliedByUserName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RepliedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoticeReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoticeReplies_Notices_NoticeId",
                        column: x => x.NoticeId,
                        principalTable: "Notices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoticeReplies_Users_RepliedByUserId",
                        column: x => x.RepliedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoticeReplies_NoticeId",
                table: "NoticeReplies",
                column: "NoticeId");

            migrationBuilder.CreateIndex(
                name: "IX_NoticeReplies_RepliedByUserId",
                table: "NoticeReplies",
                column: "RepliedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NoticeReplies_RepliedDate",
                table: "NoticeReplies",
                column: "RepliedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_Category",
                table: "Notices",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_PostedByUserId",
                table: "Notices",
                column: "PostedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_PostedDate",
                table: "Notices",
                column: "PostedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_Priority",
                table: "Notices",
                column: "Priority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoticeReplies");

            migrationBuilder.DropTable(
                name: "Notices");
        }
    }
}
