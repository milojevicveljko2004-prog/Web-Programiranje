using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LekarBolnica.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bolnice",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lokacija = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojOdeljenja = table.Column<int>(type: "int", nullable: false),
                    BrojOsoblja = table.Column<int>(type: "int", nullable: false),
                    BrojTelefona = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bolnice", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Lekari",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumRodjenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumDiplomiranja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumDobijanjaLicence = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lekari", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LekariUBolnici",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumPOtpisivanjaUgovora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Specijalnost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bolnicaID = table.Column<int>(type: "int", nullable: true),
                    lekarID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LekariUBolnici", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LekariUBolnici_Bolnice_bolnicaID",
                        column: x => x.bolnicaID,
                        principalTable: "Bolnice",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LekariUBolnici_Lekari_lekarID",
                        column: x => x.lekarID,
                        principalTable: "Lekari",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LekariUBolnici_bolnicaID",
                table: "LekariUBolnici",
                column: "bolnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_LekariUBolnici_lekarID",
                table: "LekariUBolnici",
                column: "lekarID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LekariUBolnici");

            migrationBuilder.DropTable(
                name: "Bolnice");

            migrationBuilder.DropTable(
                name: "Lekari");
        }
    }
}
