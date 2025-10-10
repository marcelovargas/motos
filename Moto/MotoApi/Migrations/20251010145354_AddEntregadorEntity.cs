using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEntregadorEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entregadores",
                columns: table => new
                {
                    Identificador = table.Column<string>(type: "varchar(50)", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumeroCnh = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TipoCnh = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    ImagemCnh = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entregadores", x => x.Identificador);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entregadores_Cnpj",
                table: "Entregadores",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entregadores_NumeroCnh",
                table: "Entregadores",
                column: "NumeroCnh",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entregadores");
        }
    }
}
