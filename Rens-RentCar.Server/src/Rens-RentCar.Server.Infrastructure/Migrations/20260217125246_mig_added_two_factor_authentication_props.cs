using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rens_RentCar.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_added_two_factor_authentication_props : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TFACode_Value",
                table: "Users",
                type: "varchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TFAConfirmCode_Value",
                table: "Users",
                type: "varchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TFAExpiresDate_Value",
                table: "Users",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TFAIscompleted_Value",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TFAStatus_Value",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TFACode_Value",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TFAConfirmCode_Value",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TFAExpiresDate_Value",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TFAIscompleted_Value",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TFAStatus_Value",
                table: "Users");
        }
    }
}
