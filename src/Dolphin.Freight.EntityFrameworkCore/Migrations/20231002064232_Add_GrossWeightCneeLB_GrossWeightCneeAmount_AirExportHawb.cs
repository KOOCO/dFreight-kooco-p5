using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Add_GrossWeightCneeLB_GrossWeightCneeAmount_AirExportHawb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GrossWeightCneeLB",
                table: "AppAirExportHawbs",
                type: "nvarchar(MAX)",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "GrossWeightCneeAmount",
                table: "AppAirExportHawbs",
                type: "nvarchar(MAX)", 
                nullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrossWeightCneeLB",
                table: "AppAirExportHawbs"
            );

            migrationBuilder.DropColumn(
                name: "GrossWeightCneeAmount",
                table: "AppAirExportHawbs"
            );
        }
    }
}
