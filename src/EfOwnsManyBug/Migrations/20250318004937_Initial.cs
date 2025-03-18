using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfOwnsManyBug.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Body_Value = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false, defaultValue: ""),
                    Body_Slices = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostSummary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Body_Value = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false, defaultValue: ""),
                    Body_Slices = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostSummary_Posts_Id",
                        column: x => x.Id,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostSummaryTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSummaryTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostSummaryTag_PostSummary_PostId",
                        column: x => x.PostId,
                        principalTable: "PostSummary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostSummaryTag_PostId_Tag",
                table: "PostSummaryTag",
                columns: new[] { "PostId", "Tag" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostSummaryTag");

            migrationBuilder.DropTable(
                name: "PostSummary");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
