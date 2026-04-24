using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCooperativeAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBCooperative",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Phones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastAccessAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PasswordChangeToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordChangeTokenExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCooperative", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBCooperativeAccessToken",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CooperativeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCooperativeAccessToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBCooperativeAccessToken_T~",
                        column: x => x.CooperativeId,
                        principalTable: "TBCooperative",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TBCooperativeRefreshToken",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CooperativeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCooperativeRefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBCooperativeRefreshToken_~",
                        column: x => x.CooperativeId,
                        principalTable: "TBCooperative",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBCooperative_Cpf",
                table: "TBCooperative",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBCooperative_Email",
                table: "TBCooperative",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBCooperativeAccessToken_C~",
                table: "TBCooperativeAccessToken",
                column: "CooperativeId");

            migrationBuilder.CreateIndex(
                name: "IX_TBCooperativeRefreshToken_~",
                table: "TBCooperativeRefreshToken",
                column: "CooperativeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBCooperativeAccessToken");

            migrationBuilder.DropTable(
                name: "TBCooperativeRefreshToken");

            migrationBuilder.DropTable(
                name: "TBCooperative");
        }
    }
}
