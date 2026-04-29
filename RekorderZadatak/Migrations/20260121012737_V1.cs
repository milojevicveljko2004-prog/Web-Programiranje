using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RekorderZadatak.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Discipline",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumOd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrenutniBrTakmicara = table.Column<int>(type: "int", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discipline", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rekorderi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DatumRodjenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pol = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Sport = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rekorderi", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rekordi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InformacijaOTakmicenju = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    rekorderID = table.Column<int>(type: "int", nullable: true),
                    disciplinaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rekordi", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rekordi_Discipline_disciplinaID",
                        column: x => x.disciplinaID,
                        principalTable: "Discipline",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Rekordi_Rekorderi_rekorderID",
                        column: x => x.rekorderID,
                        principalTable: "Rekorderi",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rekordi_disciplinaID",
                table: "Rekordi",
                column: "disciplinaID");

            migrationBuilder.CreateIndex(
                name: "IX_Rekordi_rekorderID",
                table: "Rekordi",
                column: "rekorderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rekordi");

            migrationBuilder.DropTable(
                name: "Discipline");

            migrationBuilder.DropTable(
                name: "Rekorderi");
        }
    }
}
