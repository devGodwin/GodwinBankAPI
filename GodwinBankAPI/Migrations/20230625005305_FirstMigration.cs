using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GodwinBankAPI.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountNumber = table.Column<string>(type: "text", nullable: false),
                    UserGhCardId = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    AccountName = table.Column<string>(type: "text", nullable: true),
                    EmailAddress = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Contact = table.Column<string>(type: "text", nullable: true),
                    AccountType = table.Column<string>(type: "text", nullable: true),
                    AccountBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    AccountStatus = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PasswordResetCode = table.Column<string>(type: "text", nullable: true),
                    ResetCodeExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountNumber);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
