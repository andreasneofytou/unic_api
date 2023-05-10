using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnicApi.Migrations
{
    /// <inheritdoc />
    public partial class Classes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubjectEntityId",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectEntityId1",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lecturer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SocialInsuranceNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lecturer_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Period",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Period", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PeriodId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subject_Period_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Period",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_SubjectEntityId",
                table: "User",
                column: "SubjectEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_User_SubjectEntityId1",
                table: "User",
                column: "SubjectEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_PeriodId",
                table: "Subject",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Subject_SubjectEntityId",
                table: "User",
                column: "SubjectEntityId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Subject_SubjectEntityId1",
                table: "User",
                column: "SubjectEntityId1",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Subject_SubjectEntityId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Subject_SubjectEntityId1",
                table: "User");

            migrationBuilder.DropTable(
                name: "Lecturer");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Period");

            migrationBuilder.DropIndex(
                name: "IX_User_SubjectEntityId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_SubjectEntityId1",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SubjectEntityId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SubjectEntityId1",
                table: "User");
        }
    }
}
