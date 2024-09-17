using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KPImanDental.Data.Migrations
{
    public partial class Added_RelationUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KpImanUserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KpImanUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KpImanUserRoles_KpImanRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "KpImanRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KpImanUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KpImanUserRoles_RoleId",
                table: "KpImanUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_KpImanUserRoles_UserId",
                table: "KpImanUserRoles",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KpImanUserRoles");
        }
    }
}
