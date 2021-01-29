using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreManager.Infrastructure.Migrations.ApplicationDb
{
    public partial class CurrencyPKTypeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItem_Currency_CurrencyCode",
                table: "LineItem");

            migrationBuilder.DropIndex(
                name: "IX_LineItem_CurrencyCode",
                table: "LineItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Currency");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "LineItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "LineItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Currency",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LineItem_CurrencyId",
                table: "LineItem",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItem_Currency_CurrencyId",
                table: "LineItem",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItem_Currency_CurrencyId",
                table: "LineItem");

            migrationBuilder.DropIndex(
                name: "IX_LineItem_CurrencyId",
                table: "LineItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Currency");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "LineItem",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Currency",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_LineItem_CurrencyCode",
                table: "LineItem",
                column: "CurrencyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItem_Currency_CurrencyCode",
                table: "LineItem",
                column: "CurrencyCode",
                principalTable: "Currency",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
