using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CShroudGateway.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class _41242141 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientPublicKey",
                table: "FastLogins");

            migrationBuilder.DropColumn(
                name: "ServerPrivateKey",
                table: "FastLogins");

            migrationBuilder.AddColumn<Guid>(
                name: "VerifiedUserId",
                table: "FastLogins",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedUserId",
                table: "FastLogins");

            migrationBuilder.AddColumn<byte[]>(
                name: "ClientPublicKey",
                table: "FastLogins",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ServerPrivateKey",
                table: "FastLogins",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
