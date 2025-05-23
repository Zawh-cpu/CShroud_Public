using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CShroudGateway.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class _34123123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Server",
                table: "ProtocolsSettings");

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolsSettings_Servers_ServerId",
                table: "ProtocolsSettings",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolsSettings_Servers_ServerId",
                table: "ProtocolsSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "Server",
                table: "ProtocolsSettings",
                type: "uuid",
                nullable: true);
        }
    }
}
