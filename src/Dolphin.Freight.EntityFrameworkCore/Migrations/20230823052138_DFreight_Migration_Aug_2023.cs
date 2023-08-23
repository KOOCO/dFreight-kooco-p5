using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class DFreight_Migration_Aug_2023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DomesticInstructionsDelOrder",
                table: "AppOceanImportHbls",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HblId",
                table: "AppContainers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageMeasureUnit",
                table: "AppContainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PackageUnitId",
                table: "AppContainers",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DomesticInstructionsDelOrder",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "HblId",
                table: "AppContainers");

            migrationBuilder.DropColumn(
                name: "PackageMeasureUnit",
                table: "AppContainers");

            migrationBuilder.DropColumn(
                name: "PackageUnitId",
                table: "AppContainers");
        }
    }
}
