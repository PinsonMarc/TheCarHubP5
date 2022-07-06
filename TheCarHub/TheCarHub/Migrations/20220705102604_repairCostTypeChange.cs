using Microsoft.EntityFrameworkCore.Migrations;

namespace TheCarHub.Migrations
{
    public partial class repairCostTypeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "RepairCost",
                table: "Cars",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RepairCost",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");
        }
    }
}