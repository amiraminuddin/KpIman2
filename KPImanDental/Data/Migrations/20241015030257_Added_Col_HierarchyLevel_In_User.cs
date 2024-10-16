using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KPImanDental.Data.Migrations
{
    public partial class Added_Col_HierarchyLevel_In_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HierarchyLevel",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HierarchyLevel",
                table: "Users");
        }
    }
}
