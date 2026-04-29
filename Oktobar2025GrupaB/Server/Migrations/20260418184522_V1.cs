using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prodavnice",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kapacitet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodavnice", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sastojci",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Debljina = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sastojci", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Hamburgeri",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prodat = table.Column<bool>(type: "bit", nullable: false),
                    ProdavnicaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hamburgeri", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Hamburgeri_Prodavnice_ProdavnicaID",
                        column: x => x.ProdavnicaID,
                        principalTable: "Prodavnice",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SastojciUHamburgeru",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    SastojakID = table.Column<int>(type: "int", nullable: true),
                    HamburgerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SastojciUHamburgeru", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SastojciUHamburgeru_Hamburgeri_HamburgerID",
                        column: x => x.HamburgerID,
                        principalTable: "Hamburgeri",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SastojciUHamburgeru_Sastojci_SastojakID",
                        column: x => x.SastojakID,
                        principalTable: "Sastojci",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hamburgeri_ProdavnicaID",
                table: "Hamburgeri",
                column: "ProdavnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_SastojciUHamburgeru_HamburgerID",
                table: "SastojciUHamburgeru",
                column: "HamburgerID");

            migrationBuilder.CreateIndex(
                name: "IX_SastojciUHamburgeru_SastojakID",
                table: "SastojciUHamburgeru",
                column: "SastojakID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SastojciUHamburgeru");

            migrationBuilder.DropTable(
                name: "Hamburgeri");

            migrationBuilder.DropTable(
                name: "Sastojci");

            migrationBuilder.DropTable(
                name: "Prodavnice");
        }
    }
}
