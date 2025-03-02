using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class EditModeloRuta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paradas_Rutas_RutaId",
                table: "Paradas");

            migrationBuilder.DropIndex(
                name: "IX_Paradas_RutaId",
                table: "Paradas");

            migrationBuilder.DropColumn(
                name: "RutaId",
                table: "Paradas");

            migrationBuilder.CreateTable(
                name: "RutasParadas",
                columns: table => new
                {
                    RutaId = table.Column<int>(type: "int", nullable: false),
                    ParadaId = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RutasParadas", x => new { x.RutaId, x.ParadaId });
                    table.ForeignKey(
                        name: "FK_RutasParadas_Paradas_ParadaId",
                        column: x => x.ParadaId,
                        principalTable: "Paradas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RutasParadas_Rutas_RutaId",
                        column: x => x.RutaId,
                        principalTable: "Rutas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RutasParadas_ParadaId",
                table: "RutasParadas",
                column: "ParadaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RutasParadas");

            migrationBuilder.AddColumn<int>(
                name: "RutaId",
                table: "Paradas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Paradas_RutaId",
                table: "Paradas",
                column: "RutaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paradas_Rutas_RutaId",
                table: "Paradas",
                column: "RutaId",
                principalTable: "Rutas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
