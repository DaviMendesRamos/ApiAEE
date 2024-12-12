using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiAEE.Migrations
{
    /// <inheritdoc />
    public partial class inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipes",
                columns: table => new
                {
                    CodEquipe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeEquipe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modalidade = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipes", x => x.CodEquipe);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    CodEvento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeEvento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalEvento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.CodEvento);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    UrlImagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cadastras",
                columns: table => new
                {
                    CodEquipe = table.Column<int>(type: "int", nullable: false),
                    CodEvento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cadastras", x => new { x.CodEquipe, x.CodEvento });
                    table.ForeignKey(
                        name: "FK_Cadastras_Equipes_CodEquipe",
                        column: x => x.CodEquipe,
                        principalTable: "Equipes",
                        principalColumn: "CodEquipe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cadastras_Eventos_CodEvento",
                        column: x => x.CodEvento,
                        principalTable: "Eventos",
                        principalColumn: "CodEvento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pertences",
                columns: table => new
                {
                    CodEquipe = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pertences", x => new { x.CodEquipe, x.ID });
                    table.ForeignKey(
                        name: "FK_Pertences_Equipes_CodEquipe",
                        column: x => x.CodEquipe,
                        principalTable: "Equipes",
                        principalColumn: "CodEquipe",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pertences_Usuarios_ID",
                        column: x => x.ID,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cadastras_CodEvento",
                table: "Cadastras",
                column: "CodEvento");

            migrationBuilder.CreateIndex(
                name: "IX_Pertences_ID",
                table: "Pertences",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cadastras");

            migrationBuilder.DropTable(
                name: "Pertences");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Equipes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
