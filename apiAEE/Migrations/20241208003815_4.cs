using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiAEE.Migrations
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemPerfil",
                table: "Usuarios");

            migrationBuilder.AddColumn<string>(
                name: "UrlImagem",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImagem",
                table: "Usuarios");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImagemPerfil",
                table: "Usuarios",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
