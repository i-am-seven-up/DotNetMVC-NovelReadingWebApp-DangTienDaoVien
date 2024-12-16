using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DangTienDaoVien.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTruyen",
                table: "UserTruyen");

            migrationBuilder.DropColumn(
                name: "ReadTime",
                table: "UserTruyen");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserTruyen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTruyen",
                table: "UserTruyen",
                columns: new[] { "Id", "UserId", "TruyenId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserTruyen_UserId",
                table: "UserTruyen",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTruyen",
                table: "UserTruyen");

            migrationBuilder.DropIndex(
                name: "IX_UserTruyen_UserId",
                table: "UserTruyen");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserTruyen");

            migrationBuilder.AddColumn<int>(
                name: "ReadTime",
                table: "UserTruyen",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTruyen",
                table: "UserTruyen",
                columns: new[] { "UserId", "TruyenId" });
        }
    }
}
