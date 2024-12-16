using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DangTienDaoVien.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReadTime",
                table: "UserTruyen",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadTime",
                table: "UserTruyen");
        }
    }
}
