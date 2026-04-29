using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OktobarTri2025.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategorije",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorije", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prodavnice",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Lokacija = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrTelefona = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodavnice", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Proizvodi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    kategorijaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proizvodi", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Proizvodi_Kategorije_kategorijaID",
                        column: x => x.kategorijaID,
                        principalTable: "Kategorije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProizvodiUProdavnici",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    Cena = table.Column<double>(type: "float", nullable: false),
                    proizvodID = table.Column<int>(type: "int", nullable: true),
                    prodavnicaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProizvodiUProdavnici", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProizvodiUProdavnici_Prodavnice_prodavnicaID",
                        column: x => x.prodavnicaID,
                        principalTable: "Prodavnice",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProizvodiUProdavnici_Proizvodi_proizvodID",
                        column: x => x.proizvodID,
                        principalTable: "Proizvodi",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proizvodi_kategorijaID",
                table: "Proizvodi",
                column: "kategorijaID");

            migrationBuilder.CreateIndex(
                name: "IX_ProizvodiUProdavnici_prodavnicaID",
                table: "ProizvodiUProdavnici",
                column: "prodavnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_ProizvodiUProdavnici_proizvodID",
                table: "ProizvodiUProdavnici",
                column: "proizvodID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProizvodiUProdavnici");

            migrationBuilder.DropTable(
                name: "Prodavnice");

            migrationBuilder.DropTable(
                name: "Proizvodi");

            migrationBuilder.DropTable(
                name: "Kategorije");
        }
    }
}
