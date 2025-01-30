using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiAEE.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
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
                    Modalidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlImagem = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UrlImagem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.CodEvento);
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
                name: "Inscricoes",
                columns: table => new
                {
                    CodEquipe = table.Column<int>(type: "int", nullable: false),
                    CodEvento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscricoes", x => new { x.CodEquipe, x.CodEvento });
                    table.ForeignKey(
                        name: "FK_Inscricoes_Equipes_CodEquipe",
                        column: x => x.CodEquipe,
                        principalTable: "Equipes",
                        principalColumn: "CodEquipe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscricoes_Eventos_CodEvento",
                        column: x => x.CodEvento,
                        principalTable: "Eventos",
                        principalColumn: "CodEvento",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "Membros",
                columns: table => new
                {
                    CodEquipe = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membros", x => new { x.CodEquipe, x.ID });
                    table.ForeignKey(
                        name: "FK_Membros_Equipes_CodEquipe",
                        column: x => x.CodEquipe,
                        principalTable: "Equipes",
                        principalColumn: "CodEquipe",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Membros_Usuarios_ID",
                        column: x => x.ID,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_Participas_Inscricoes_CodEquipe_CodEvento",
                        columns: x => new { x.CodEquipe, x.CodEvento },
                        principalTable: "Inscricoes",
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
                name: "IX_Inscricoes_CodEvento",
                table: "Inscricoes",
                column: "CodEvento");

            migrationBuilder.CreateIndex(
                name: "IX_Membros_ID",
                table: "Membros",
                column: "ID");

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
                name: "Membros");

            migrationBuilder.DropTable(
                name: "Midias");

            migrationBuilder.DropTable(
                name: "Participas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Inscricoes");

            migrationBuilder.DropTable(
                name: "Partidas");

            migrationBuilder.DropTable(
                name: "Equipes");

            migrationBuilder.DropTable(
                name: "Eventos");
        }
    }
}
