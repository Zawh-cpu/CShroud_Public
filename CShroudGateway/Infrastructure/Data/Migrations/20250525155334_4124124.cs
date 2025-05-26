using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CShroudGateway.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class _4124124 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FastLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ipv4Address = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    LoginTimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    DeviceInfo = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: true),
                    Variants = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastLogins", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mails_SenderId",
                table: "Mails",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mails_Users_SenderId",
                table: "Mails",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mails_Users_SenderId",
                table: "Mails");

            migrationBuilder.DropTable(
                name: "FastLogins");

            migrationBuilder.DropIndex(
                name: "IX_Mails_SenderId",
                table: "Mails");
        }
    }
}
