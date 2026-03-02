using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rens_RentCar.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edited_DrivingLicenseIssueDate_property_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DrivingLicenceIssuanceDate_Value",
                table: "Customers",
                newName: "DrivingLicenseIssueDate_Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DrivingLicenseIssueDate_Value",
                table: "Customers",
                newName: "DrivingLicenceIssuanceDate_Value");
        }
    }
}
