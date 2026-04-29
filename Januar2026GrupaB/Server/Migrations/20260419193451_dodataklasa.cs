using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class dodataklasa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SastojciUProdavnici_Mesta_MestoID",
                table: "SastojciUProdavnici");

            migrationBuilder.DropIndex(
                name: "IX_SastojciUProdavnici_MestoID",
                table: "SastojciUProdavnici");

            migrationBuilder.DropColumn(
                name: "MestoID",
                table: "SastojciUProdavnici");

            migrationBuilder.CreateTable(
                name: "SastojciUSendvicu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    MestoID = table.Column<int>(type: "int", nullable: true),
                    SastojakUProdavniciID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SastojciUSendvicu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SastojciUSendvicu_Mesta_MestoID",
                        column: x => x.MestoID,
                        principalTable: "Mesta",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SastojciUSendvicu_SastojciUProdavnici_SastojakUProdavniciID",
                        column: x => x.SastojakUProdavniciID,
                        principalTable: "SastojciUProdavnici",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SastojciUSendvicu_MestoID",
                table: "SastojciUSendvicu",
                column: "MestoID");

            migrationBuilder.CreateIndex(
                name: "IX_SastojciUSendvicu_SastojakUProdavniciID",
                table: "SastojciUSendvicu",
                column: "SastojakUProdavniciID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SastojciUSendvicu");

            migrationBuilder.AddColumn<int>(
                name: "MestoID",
                table: "SastojciUProdavnici",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SastojciUProdavnici_MestoID",
                table: "SastojciUProdavnici",
                column: "MestoID");

            migrationBuilder.AddForeignKey(
                name: "FK_SastojciUProdavnici_Mesta_MestoID",
                table: "SastojciUProdavnici",
                column: "MestoID",
                principalTable: "Mesta",
                principalColumn: "ID");
        }
    }
}
