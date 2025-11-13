using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExtractInfoIdentityDocument.Migrations
{
    /// <inheritdoc />
    public partial class EditSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_Name",
                table: "Subscriptions",
                newName: "IX_Subscription_Name");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_IsDefault_True",
                table: "Subscriptions",
                column: "IsDefault",
                unique: true,
                filter: "[IsDefault] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscription_IsDefault_True",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Subscriptions");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_Name",
                table: "Subscriptions",
                newName: "IX_Subscriptions_Name");
        }
    }
}
