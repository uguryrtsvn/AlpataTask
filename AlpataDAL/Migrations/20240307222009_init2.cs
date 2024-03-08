using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlpataDAL.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_AppUsers_AppUserId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_AppUserId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Meetings");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorUserId",
                table: "Meetings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_CreatorUserId",
                table: "Meetings",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_AppUsers_CreatorUserId",
                table: "Meetings",
                column: "CreatorUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_AppUsers_CreatorUserId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_CreatorUserId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Meetings");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Meetings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_AppUserId",
                table: "Meetings",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_AppUsers_AppUserId",
                table: "Meetings",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
