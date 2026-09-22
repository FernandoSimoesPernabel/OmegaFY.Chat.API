using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmegaFY.Chat.API.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Members_ConversationId_UserId",
                table: "Members",
                columns: new[] { "ConversationId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberMessages_MessageId_SenderMemberId_DestinationMemberId",
                table: "MemberMessages",
                columns: new[] { "MessageId", "SenderMemberId", "DestinationMemberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_RequestingUserId_InvitedUserId",
                table: "Friendships",
                columns: new[] { "RequestingUserId", "InvitedUserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_ConversationId_UserId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_MemberMessages_MessageId_SenderMemberId_DestinationMemberId",
                table: "MemberMessages");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_RequestingUserId_InvitedUserId",
                table: "Friendships");
        }
    }
}