using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RateYourMusicApp.Migrations
{
    public partial class SongPictureFile1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profilePicture",
                table: "Song",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profilePicture",
                table: "Song");
        }
    }
}
