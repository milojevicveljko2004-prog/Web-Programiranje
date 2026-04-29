using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaterijalStovariste.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materijali",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sifra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cena = table.Column<float>(type: "real", nullable: false),
                    NazivProizvodjaca = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materijali", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Stovarista",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojTelefona = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stovarista", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IsporuceniMaterijali",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumIsporuke = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KolicinaMaterijala = table.Column<int>(type: "int", nullable: false),
                    materijalID = table.Column<int>(type: "int", nullable: true),
                    stovaristeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IsporuceniMaterijali", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IsporuceniMaterijali_Materijali_materijalID",
                        column: x => x.materijalID,
                        principalTable: "Materijali",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_IsporuceniMaterijali_Stovarista_stovaristeID",
                        column: x => x.stovaristeID,
                        principalTable: "Stovarista",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IsporuceniMaterijali_materijalID",
                table: "IsporuceniMaterijali",
                column: "materijalID");

            migrationBuilder.CreateIndex(
                name: "IX_IsporuceniMaterijali_stovaristeID",
                table: "IsporuceniMaterijali",
                column: "stovaristeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IsporuceniMaterijali");

            migrationBuilder.DropTable(
                name: "Materijali");

            migrationBuilder.DropTable(
                name: "Stovarista");
        }
    }
}
