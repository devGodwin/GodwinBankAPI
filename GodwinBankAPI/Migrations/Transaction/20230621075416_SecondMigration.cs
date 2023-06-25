using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GodwinBankAPI.Migrations.Transaction
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<string>(type: "text", nullable: false),
                    TransactionType = table.Column<string>(type: "text", nullable: true),
                    AccountBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    SourceAccountNumber = table.Column<string>(type: "text", nullable: true),
                    DestinationAccountNumber = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TransactionStatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
