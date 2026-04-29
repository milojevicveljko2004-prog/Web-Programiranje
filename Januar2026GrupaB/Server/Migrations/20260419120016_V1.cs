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
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
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
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sastojci", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Mesta",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProdavnicaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesta", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Mesta_Prodavnice_ProdavnicaID",
                        column: x => x.ProdavnicaID,
                        principalTable: "Prodavnice",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SastojciUProdavnici",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    ProdavnicaID = table.Column<int>(type: "int", nullable: true),
                    SastojakID = table.Column<int>(type: "int", nullable: true),
                    MestoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SastojciUProdavnici", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SastojciUProdavnici_Mesta_MestoID",
                        column: x => x.MestoID,
                        principalTable: "Mesta",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SastojciUProdavnici_Prodavnice_ProdavnicaID",
                        column: x => x.ProdavnicaID,
                        principalTable: "Prodavnice",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SastojciUProdavnici_Sastojci_SastojakID",
                        column: x => x.SastojakID,
                        principalTable: "Sastojci",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mesta_ProdavnicaID",
                table: "Mesta",
                column: "ProdavnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_SastojciUProdavnici_MestoID",
                table: "SastojciUProdavnici",
                column: "MestoID");

            migrationBuilder.CreateIndex(
                name: "IX_SastojciUProdavnici_ProdavnicaID",
                table: "SastojciUProdavnici",
                column: "ProdavnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_SastojciUProdavnici_SastojakID",
                table: "SastojciUProdavnici",
                column: "SastojakID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SastojciUProdavnici");

            migrationBuilder.DropTable(
                name: "Mesta");

            migrationBuilder.DropTable(
                name: "Sastojci");

            migrationBuilder.DropTable(
                name: "Prodavnice");
        }
    }
}
