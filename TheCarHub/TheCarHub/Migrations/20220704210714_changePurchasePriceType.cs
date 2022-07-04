using Microsoft.EntityFrameworkCore.Migrations;

namespace TheCarHub.Migrations
{
    public partial class changePurchasePriceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PurchasePrice",
                table: "Cars",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "PurchasePrice",
                table: "Cars",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");
        }
    }
}
