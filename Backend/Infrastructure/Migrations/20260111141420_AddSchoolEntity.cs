using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSchoolEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SchoolName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SchoolCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    District = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Block = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Village = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SchoolType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ManagementType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TotalStudents = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalTeachers = table.Column<int>(type: "INTEGER", nullable: false),
                    PrincipalName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ContactNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    EstablishedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schools_Block",
                table: "Schools",
                column: "Block");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_District",
                table: "Schools",
                column: "District");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SchoolCode",
                table: "Schools",
                column: "SchoolCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}
