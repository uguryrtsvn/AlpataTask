using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlpataDAL.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MeetingId",
                table: "AppUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_MeetingId",
                table: "AppUsers",
                column: "MeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Meetings_MeetingId",
                table: "AppUsers",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Meetings_MeetingId",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_MeetingId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "AppUsers");
        }
    }
}
