using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiAEE.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pertences_Usuarios_CodUsuario",
                table: "Pertences");

            migrationBuilder.DropColumn(
                name: "Aceito",
                table: "Pertences");

            migrationBuilder.RenameColumn(
                name: "CodUsuario",
                table: "Pertences",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Pertences_CodUsuario",
                table: "Pertences",
                newName: "IX_Pertences_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pertences_Usuarios_ID",
                table: "Pertences",
                column: "ID",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pertences_Usuarios_ID",
                table: "Pertences");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Pertences",
                newName: "CodUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_Pertences_ID",
                table: "Pertences",
                newName: "IX_Pertences_CodUsuario");

            migrationBuilder.AddColumn<bool>(
                name: "Aceito",
                table: "Pertences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Pertences_Usuarios_CodUsuario",
                table: "Pertences",
                column: "CodUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
