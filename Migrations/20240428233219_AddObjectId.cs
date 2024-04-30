using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webhook_endpoint_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class AddObjectId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebhookObjectId",
                table: "WebhookEvents",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebhookObjectId",
                table: "WebhookEvents");
        }
    }
}
