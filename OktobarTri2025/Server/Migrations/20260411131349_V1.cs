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
                name: "Boje",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boje", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Fabrike",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BrKontejnera = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fabrike", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kontejneri",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxKapacitet = table.Column<int>(type: "int", nullable: false),
                    TrenutniKapacitet = table.Column<int>(type: "int", nullable: false),
                    BojaID = table.Column<int>(type: "int", nullable: true),
                    FabrikaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kontejneri", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Kontejneri_Boje_BojaID",
                        column: x => x.BojaID,
                        principalTable: "Boje",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Kontejneri_Fabrike_FabrikaID",
                        column: x => x.FabrikaID,
                        principalTable: "Fabrike",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kontejneri_BojaID",
                table: "Kontejneri",
                column: "BojaID");

            migrationBuilder.CreateIndex(
                name: "IX_Kontejneri_FabrikaID",
                table: "Kontejneri",
                column: "FabrikaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kontejneri");

            migrationBuilder.DropTable(
                name: "Boje");

            migrationBuilder.DropTable(
                name: "Fabrike");
        }
    }
}
