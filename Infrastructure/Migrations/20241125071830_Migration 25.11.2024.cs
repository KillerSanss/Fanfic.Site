using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration25112024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_parent_comment_id",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_parent_comment_id",
                table: "Comments",
                column: "parent_comment_id",
                principalTable: "Comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_parent_comment_id",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_parent_comment_id",
                table: "Comments",
                column: "parent_comment_id",
                principalTable: "Comments",
                principalColumn: "id");
        }
    }
}
