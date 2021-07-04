using Microsoft.EntityFrameworkCore.Migrations;

namespace api.data
{
    public partial class TokBoxTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Session_SessionIdId",
                table: "Tokens");

            migrationBuilder.RenameColumn(
                name: "SessionIdId",
                table: "Tokens",
                newName: "tokboxSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Tokens_SessionIdId",
                table: "Tokens",
                newName: "IX_Tokens_tokboxSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Session_tokboxSessionId",
                table: "Tokens",
                column: "tokboxSessionId",
                principalTable: "Session",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Session_tokboxSessionId",
                table: "Tokens");

            migrationBuilder.RenameColumn(
                name: "tokboxSessionId",
                table: "Tokens",
                newName: "SessionIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Tokens_tokboxSessionId",
                table: "Tokens",
                newName: "IX_Tokens_SessionIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Session_SessionIdId",
                table: "Tokens",
                column: "SessionIdId",
                principalTable: "Session",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
