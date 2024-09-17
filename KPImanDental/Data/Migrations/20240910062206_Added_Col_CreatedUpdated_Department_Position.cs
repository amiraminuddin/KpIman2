using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KPImanDental.Data.Migrations
{
    public partial class Added_Col_CreatedUpdated_Department_Position : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "KpImanPosition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "KpImanPosition",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "KpImanPosition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "KpImanPosition",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "KpImanDepartment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "KpImanDepartment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "KpImanDepartment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "KpImanDepartment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "KpImanPosition");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "KpImanPosition");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "KpImanPosition");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "KpImanPosition");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "KpImanDepartment");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "KpImanDepartment");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "KpImanDepartment");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "KpImanDepartment");
        }
    }
}
