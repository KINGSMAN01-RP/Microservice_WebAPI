using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialAuthDBConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleDetails = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RoleAccessLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", maxLength: 100, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_RefreshToken_tbl_User_UserId",
                        column: x => x.UserId,
                        principalTable: "tbl_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RefreshToken_UserId",
                table: "tbl_RefreshToken",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_RefreshToken");

            migrationBuilder.DropTable(
                name: "tbl_Role");

            migrationBuilder.DropTable(
                name: "tbl_User");
        }
    }
}
