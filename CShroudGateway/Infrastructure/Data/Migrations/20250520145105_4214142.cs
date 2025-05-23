using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CShroudGateway.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class _4214142 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Protocols");

            migrationBuilder.CreateTable(
                name: "ProtocolsSettings",
                columns: table => new
                {
                    Protocol = table.Column<int>(type: "integer", nullable: false),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Server = table.Column<Guid>(type: "uuid", nullable: true),
                    Port = table.Column<long>(type: "bigint", nullable: false),
                    ExtraContent = table.Column<JsonDocument>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolsSettings", x => new { x.ServerId, x.Protocol });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProtocolsSettings");

            migrationBuilder.CreateTable(
                name: "Protocols",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Port = table.Column<long>(type: "bigint", nullable: false),
                    PublicKey = table.Column<string>(type: "text", nullable: false),
                    URIArgs = table.Column<JsonDocument>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protocols", x => x.Id);
                });
        }
    }
}
