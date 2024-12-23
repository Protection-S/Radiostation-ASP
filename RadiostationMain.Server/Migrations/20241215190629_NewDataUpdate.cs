using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RadiostationMain.Server.Migrations
{
    /// <inheritdoc />
    public partial class NewDataUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Stations_StationId",
                table: "Tracks");

            migrationBuilder.RenameColumn(
                name: "StationId",
                table: "Tracks",
                newName: "PlaylistId");

            migrationBuilder.RenameIndex(
                name: "IX_Tracks_StationId",
                table: "Tracks",
                newName: "IX_Tracks_PlaylistId");

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    StationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Playlists_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_StationId",
                table: "Playlists",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Playlists_PlaylistId",
                table: "Tracks",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Playlists_PlaylistId",
                table: "Tracks");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.RenameColumn(
                name: "PlaylistId",
                table: "Tracks",
                newName: "StationId");

            migrationBuilder.RenameIndex(
                name: "IX_Tracks_PlaylistId",
                table: "Tracks",
                newName: "IX_Tracks_StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Stations_StationId",
                table: "Tracks",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
