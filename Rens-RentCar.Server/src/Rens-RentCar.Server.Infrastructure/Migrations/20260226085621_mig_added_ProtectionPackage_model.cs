using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rens_RentCar.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_added_ProtectionPackage_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProtectionPackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Price_Value = table.Column<decimal>(type: "money", nullable: false),
                    IsRecommended_Value = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionPackages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionCoverage",
                columns: table => new
                {
                    ProtectionPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionCoverage", x => new { x.ProtectionPackageId, x.Id });
                    table.ForeignKey(
                        name: "FK_ProtectionCoverage_ProtectionPackages_ProtectionPackageId",
                        column: x => x.ProtectionPackageId,
                        principalTable: "ProtectionPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProtectionCoverage");

            migrationBuilder.DropTable(
                name: "ProtectionPackages");
        }
    }
}
