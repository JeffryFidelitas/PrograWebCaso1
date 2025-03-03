using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class ModeloRutaArregloRelaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horario_Rutas_RutaId",
                table: "Horario");

            migrationBuilder.DropForeignKey(
                name: "FK_Parada_Rutas_RutaId",
                table: "Parada");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parada",
                table: "Parada");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Horario",
                table: "Horario");

            migrationBuilder.DropIndex(
                name: "IX_Horario_RutaId",
                table: "Horario");

            migrationBuilder.DropColumn(
                name: "RutaId",
                table: "Horario");

            migrationBuilder.RenameTable(
                name: "Parada",
                newName: "Paradas");

            migrationBuilder.RenameTable(
                name: "Horario",
                newName: "Horarios");

            migrationBuilder.RenameIndex(
                name: "IX_Parada_RutaId",
                table: "Paradas",
                newName: "IX_Paradas_RutaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Paradas",
                table: "Paradas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Horarios",
                table: "Horarios",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RutasHorarios",
                columns: table => new
                {
                    RutaId = table.Column<int>(type: "int", nullable: false),
                    HorarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RutasHorarios", x => new { x.RutaId, x.HorarioId });
                    table.ForeignKey(
                        name: "FK_RutasHorarios_Horarios_HorarioId",
                        column: x => x.HorarioId,
                        principalTable: "Horarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RutasHorarios_Rutas_RutaId",
                        column: x => x.RutaId,
                        principalTable: "Rutas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RutasHorarios_HorarioId",
                table: "RutasHorarios",
                column: "HorarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paradas_Rutas_RutaId",
                table: "Paradas",
                column: "RutaId",
                principalTable: "Rutas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paradas_Rutas_RutaId",
                table: "Paradas");

            migrationBuilder.DropTable(
                name: "RutasHorarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Paradas",
                table: "Paradas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Horarios",
                table: "Horarios");

            migrationBuilder.RenameTable(
                name: "Paradas",
                newName: "Parada");

            migrationBuilder.RenameTable(
                name: "Horarios",
                newName: "Horario");

            migrationBuilder.RenameIndex(
                name: "IX_Paradas_RutaId",
                table: "Parada",
                newName: "IX_Parada_RutaId");

            migrationBuilder.AddColumn<int>(
                name: "RutaId",
                table: "Horario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parada",
                table: "Parada",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Horario",
                table: "Horario",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Horario_RutaId",
                table: "Horario",
                column: "RutaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Horario_Rutas_RutaId",
                table: "Horario",
                column: "RutaId",
                principalTable: "Rutas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parada_Rutas_RutaId",
                table: "Parada",
                column: "RutaId",
                principalTable: "Rutas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
