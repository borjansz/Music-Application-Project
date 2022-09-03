using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RateYourMusicApp.Migrations
{
    public partial class YTurl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SongUrl",
                table: "Song",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SongUrl",
                table: "Song");
        }
    }
}
