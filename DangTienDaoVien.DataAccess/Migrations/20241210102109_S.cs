using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DangTienDaoVien.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class S : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "TotallReadTime",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "None",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "None");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "None",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "None");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TotallReadTime",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "None",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "None");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "None",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "None");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
