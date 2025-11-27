using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExtractInfoIdentityDocument.Migrations
{
    /// <inheritdoc />
    public partial class AddLogicDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Uses",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "IdentityCards",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Uses");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "IdentityCards");
        }
    }
}
