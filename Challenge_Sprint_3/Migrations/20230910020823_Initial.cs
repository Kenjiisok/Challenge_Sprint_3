using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Challenge_Sprint_3.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    email = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "RAW(2000)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "RAW(2000)", nullable: false),
                    VerficationToken = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
