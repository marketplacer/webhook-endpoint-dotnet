using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webhook_endpoint_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class AddWebhookEventType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebhookEventType",
                table: "WebhookEvents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebhookObjectType",
                table: "WebhookEvents",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebhookEventType",
                table: "WebhookEvents");

            migrationBuilder.DropColumn(
                name: "WebhookObjectType",
                table: "WebhookEvents");
        }
    }
}
