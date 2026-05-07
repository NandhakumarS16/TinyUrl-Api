using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinyUrl.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TinyUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalUrl = table.Column<string>(type: "nvarchar(2048)", nullable: false),
                    ShortCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Clicks = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinyUrls", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TinyUrls_ShortCode",
                table: "TinyUrls",
                column: "ShortCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TinyUrls");
        }
    }
}
