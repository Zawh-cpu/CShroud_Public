using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CShroudGateway.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class _4124214 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FastLogins");

            migrationBuilder.RenameColumn(
                name: "LoginTimeStamp",
                table: "FastLogins",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<long[]>(
                name: "Variants",
                table: "FastLogins",
                type: "bigint[]",
                nullable: false,
                oldClrType: typeof(int[]),
                oldType: "integer[]");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "FastLogins",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<byte[]>(
                name: "ClientPublicKey",
                table: "FastLogins",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Secret",
                table: "FastLogins",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "ServerPrivateKey",
                table: "FastLogins",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientPublicKey",
                table: "FastLogins");

            migrationBuilder.DropColumn(
                name: "Secret",
                table: "FastLogins");

            migrationBuilder.DropColumn(
                name: "ServerPrivateKey",
                table: "FastLogins");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "FastLogins",
                newName: "LoginTimeStamp");

            migrationBuilder.AlterColumn<int[]>(
                name: "Variants",
                table: "FastLogins",
                type: "integer[]",
                nullable: false,
                oldClrType: typeof(long[]),
                oldType: "bigint[]");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "FastLogins",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "FastLogins",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
