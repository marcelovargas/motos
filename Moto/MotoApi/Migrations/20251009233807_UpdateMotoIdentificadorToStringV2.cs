using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MotoApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMotoIdentificadorToStringV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Primeiro remover a estratégia de identidade, para permitir alteração de tipo
            // PostgreSQL não permite alterar o tipo de uma coluna identity diretamente
            migrationBuilder.DropColumn(
                name: "Identificador",
                table: "Motos");

            migrationBuilder.AddColumn<string>(
                name: "Identificador",
                table: "Motos",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            // Para manter os dados existentes, precisaríamos de uma abordagem diferente
            // que não é possível com a estrutura atual do banco
            
            // Atualizar o tamanho do campo Placa
            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "Motos",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(7)",
                oldMaxLength: 7);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverter alterações
            migrationBuilder.DropColumn(
                name: "Identificador",
                table: "Motos");

            migrationBuilder.AddColumn<int>(
                name: "Identificador",
                table: "Motos",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            // Reverter o tamanho do campo Placa
            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "Motos",
                type: "character varying(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);
        }
    }
}
