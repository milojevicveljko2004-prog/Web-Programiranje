using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KlkBiblioteka.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Biblioteke",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAdresa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biblioteke", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Knjige",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naslov = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImeAutora = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GodinaIzdavanja = table.Column<int>(type: "int", nullable: false),
                    NazivIzdavaca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrojUEvidencijiBiblioteke = table.Column<int>(type: "int", nullable: false),
                    bibliotekaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knjige", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Knjige_Biblioteke_bibliotekaID",
                        column: x => x.bibliotekaID,
                        principalTable: "Biblioteke",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Izdavanja",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumVracanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bibliotekaID = table.Column<int>(type: "int", nullable: true),
                    knjigaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izdavanja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Izdavanja_Biblioteke_bibliotekaID",
                        column: x => x.bibliotekaID,
                        principalTable: "Biblioteke",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Izdavanja_Knjige_knjigaID",
                        column: x => x.knjigaID,
                        principalTable: "Knjige",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Izdavanja_bibliotekaID",
                table: "Izdavanja",
                column: "bibliotekaID");

            migrationBuilder.CreateIndex(
                name: "IX_Izdavanja_knjigaID",
                table: "Izdavanja",
                column: "knjigaID");

            migrationBuilder.CreateIndex(
                name: "IX_Knjige_bibliotekaID",
                table: "Knjige",
                column: "bibliotekaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Izdavanja");

            migrationBuilder.DropTable(
                name: "Knjige");

            migrationBuilder.DropTable(
                name: "Biblioteke");
        }
    }
}
