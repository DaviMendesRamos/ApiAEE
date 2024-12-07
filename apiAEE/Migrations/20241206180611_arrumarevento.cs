using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiAEE.Migrations
{
    /// <inheritdoc />
    public partial class arrumarevento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipes_Usuarios_CriadorId",
                table: "Equipes");

            migrationBuilder.DropIndex(
                name: "IX_Equipes_CriadorId",
                table: "Equipes");

            migrationBuilder.DropColumn(
                name: "CriadorId",
                table: "Equipes");

            migrationBuilder.RenameColumn(
                name: "NomeJogadores",
                table: "Equipes",
                newName: "Modalidade");

            migrationBuilder.RenameColumn(
                name: "CodEquipes",
                table: "Equipes",
                newName: "CodEquipe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Modalidade",
                table: "Equipes",
                newName: "NomeJogadores");

            migrationBuilder.RenameColumn(
                name: "CodEquipe",
                table: "Equipes",
                newName: "CodEquipes");

            migrationBuilder.AddColumn<int>(
                name: "CriadorId",
                table: "Equipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Equipes_CriadorId",
                table: "Equipes",
                column: "CriadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipes_Usuarios_CriadorId",
                table: "Equipes",
                column: "CriadorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
