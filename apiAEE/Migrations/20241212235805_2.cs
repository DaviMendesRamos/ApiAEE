using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiAEE.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlImagem",
                table: "Eventos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlImagem",
                table: "Equipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Midias",
                columns: table => new
                {
                    CodMidia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlImagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodEvento = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midias", x => x.CodMidia);
                    table.ForeignKey(
                        name: "FK_Midias_Eventos_CodEvento",
                        column: x => x.CodEvento,
                        principalTable: "Eventos",
                        principalColumn: "CodEvento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Partidas",
                columns: table => new
                {
                    CodPartida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pontuacao = table.Column<int>(type: "int", nullable: false),
                    QuantMinJogadores = table.Column<int>(type: "int", nullable: false),
                    HoratioPartida = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidas", x => x.CodPartida);
                });

            migrationBuilder.CreateTable(
                name: "Amistososos",
                columns: table => new
                {
                    CodEquipe = table.Column<int>(type: "int", nullable: false),
                    CodPartida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amistososos", x => new { x.CodEquipe, x.CodPartida });
                    table.ForeignKey(
                        name: "FK_Amistososos_Equipes_CodEquipe",
                        column: x => x.CodEquipe,
                        principalTable: "Equipes",
                        principalColumn: "CodEquipe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Amistososos_Partidas_CodPartida",
                        column: x => x.CodPartida,
                        principalTable: "Partidas",
                        principalColumn: "CodPartida",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participas",
                columns: table => new
                {
                    CodEquipe = table.Column<int>(type: "int", nullable: false),
                    CodEvento = table.Column<int>(type: "int", nullable: false),
                    CodPartida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participas", x => new { x.CodEquipe, x.CodEvento, x.CodPartida });
                    table.ForeignKey(
                        name: "FK_Participas_Cadastras_CodEquipe_CodEvento",
                        columns: x => new { x.CodEquipe, x.CodEvento },
                        principalTable: "Cadastras",
                        principalColumns: new[] { "CodEquipe", "CodEvento" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participas_Partidas_CodPartida",
                        column: x => x.CodPartida,
                        principalTable: "Partidas",
                        principalColumn: "CodPartida",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amistososos_CodPartida",
                table: "Amistososos",
                column: "CodPartida");

            migrationBuilder.CreateIndex(
                name: "IX_Midias_CodEvento",
                table: "Midias",
                column: "CodEvento");

            migrationBuilder.CreateIndex(
                name: "IX_Participas_CodPartida",
                table: "Participas",
                column: "CodPartida");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amistososos");

            migrationBuilder.DropTable(
                name: "Midias");

            migrationBuilder.DropTable(
                name: "Participas");

            migrationBuilder.DropTable(
                name: "Partidas");

            migrationBuilder.DropColumn(
                name: "UrlImagem",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "UrlImagem",
                table: "Equipes");
        }
    }
}
