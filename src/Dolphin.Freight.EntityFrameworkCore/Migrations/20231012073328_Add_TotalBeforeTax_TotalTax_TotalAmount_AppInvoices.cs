using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Add_TotalBeforeTax_TotalTax_TotalAmount_AppInvoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalBeforeTax",
                table: "AppInvoices",
                type: "float",
                nullable: true
            );
            migrationBuilder.AddColumn<double>(
                name: "TotalTax",
                table: "AppInvoices",
                type: "float",
                nullable: true
            );
            migrationBuilder.AddColumn<double>(
                name: "TotalAmount",
                table: "AppInvoices",
                type: "float",
                nullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalBeforeTax",
                table: "AppInvoices"
            );
            migrationBuilder.DropColumn(
                name: "TotalTax",
                table: "AppInvoices"
            );
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "AppInvoices"
            );
        }
    }
}
