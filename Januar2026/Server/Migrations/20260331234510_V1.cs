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
                name: "Artikli",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brend = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SifraModela = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Slika = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikli", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prodavnice",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodavnice", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ArtikliUProdaji",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cena = table.Column<double>(type: "float", nullable: false),
                    KolicinaS = table.Column<int>(type: "int", nullable: false),
                    KolicinaM = table.Column<int>(type: "int", nullable: false),
                    KolicinaL = table.Column<int>(type: "int", nullable: false),
                    ArtikalID = table.Column<int>(type: "int", nullable: true),
                    ProdavnicaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtikliUProdaji", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArtikliUProdaji_Artikli_ArtikalID",
                        column: x => x.ArtikalID,
                        principalTable: "Artikli",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ArtikliUProdaji_Prodavnice_ProdavnicaID",
                        column: x => x.ProdavnicaID,
                        principalTable: "Prodavnice",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtikliUProdaji_ArtikalID",
                table: "ArtikliUProdaji",
                column: "ArtikalID");

            migrationBuilder.CreateIndex(
                name: "IX_ArtikliUProdaji_ProdavnicaID",
                table: "ArtikliUProdaji",
                column: "ProdavnicaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtikliUProdaji");

            migrationBuilder.DropTable(
                name: "Artikli");

            migrationBuilder.DropTable(
                name: "Prodavnice");
        }
    }
}
