using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DangTienDaoVien.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class @do : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "bookmark",
                table: "UserTruyen",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "hasRated",
                table: "Comment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "rating",
                table: "Comment",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bookmark",
                table: "UserTruyen");

            migrationBuilder.DropColumn(
                name: "hasRated",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "rating",
                table: "Comment");
        }
    }
}
