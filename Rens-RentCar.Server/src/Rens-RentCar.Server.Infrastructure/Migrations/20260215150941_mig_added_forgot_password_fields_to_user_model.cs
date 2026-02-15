using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rens_RentCar.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_added_forgot_password_fields_to_user_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ForgotPasswordDate_Value",
                table: "Users",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ForgotPasswordId_Value",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsForgotPasswordCompleted_Value",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForgotPasswordDate_Value",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ForgotPasswordId_Value",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsForgotPasswordCompleted_Value",
                table: "Users");
        }
    }
}
