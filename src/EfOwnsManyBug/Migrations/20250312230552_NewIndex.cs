using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfOwnsManyBug.Migrations
{
    /// <inheritdoc />
    public partial class NewIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostTag_PostId",
                table: "PostTag");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_PostId_Tag",
                table: "PostTag",
                columns: new[] { "PostId", "Tag" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostTag_PostId_Tag",
                table: "PostTag");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_PostId",
                table: "PostTag",
                column: "PostId");
        }
    }
}
