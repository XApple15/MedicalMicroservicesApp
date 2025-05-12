using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentService.Migrations
{
    /// <inheritdoc />
    public partial class addedconsultationparams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConsultationDate",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Symptoms",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Treatment",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsultationDate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Symptoms",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Treatment",
                table: "Appointments");
        }
    }
}
