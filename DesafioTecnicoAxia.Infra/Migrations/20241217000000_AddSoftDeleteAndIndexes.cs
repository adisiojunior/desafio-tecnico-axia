using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioTecnicoAxia.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteAndIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Veiculos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Veiculos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Marca",
                table: "Veiculos",
                column: "Marca");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Modelo",
                table: "Veiculos",
                column: "Modelo");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Valor",
                table: "Veiculos",
                column: "Valor");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_CreatedAt",
                table: "Veiculos",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_IsDeleted_Marca",
                table: "Veiculos",
                columns: new[] { "IsDeleted", "Marca" });

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_IsDeleted_Valor",
                table: "Veiculos",
                columns: new[] { "IsDeleted", "Valor" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Veiculos_IsDeleted_Valor",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_IsDeleted_Marca",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_CreatedAt",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_Valor",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_Modelo",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_Marca",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Veiculos");
        }
    }
}

