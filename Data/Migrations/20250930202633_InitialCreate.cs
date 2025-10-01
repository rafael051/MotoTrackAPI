using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoTrackAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_FILIAL",
                columns: table => new
                {
                    ID_FILIAL = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_FILIAL = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    DS_ENDERECO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    DS_BAIRRO = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: true),
                    DS_CIDADE = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: true),
                    DS_ESTADO = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: true),
                    NR_CEP = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    VL_LATITUDE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    VL_LONGITUDE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    RAIO_GEOFENCE_M = table.Column<double>(type: "BINARY_DOUBLE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_FILIAL", x => x.ID_FILIAL);
                });

            migrationBuilder.CreateTable(
                name: "TB_MOTO",
                columns: table => new
                {
                    ID_MOTO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CD_PLACA = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    NM_MODELO = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    NM_MARCA = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    NR_ANO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DS_STATUS = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: false),
                    ID_FILIAL = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    VL_LATITUDE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    VL_LONGITUDE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    DT_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, defaultValueSql: "SYSTIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MOTO", x => x.ID_MOTO);
                    table.ForeignKey(
                        name: "FK_TB_MOTO_TB_FILIAL_ID_FILIAL",
                        column: x => x.ID_FILIAL,
                        principalTable: "TB_FILIAL",
                        principalColumn: "ID_FILIAL",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_USUARIO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    DS_EMAIL = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    DS_SENHA = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    TP_PERFIL = table.Column<string>(type: "NVARCHAR2(40)", maxLength: 40, nullable: false),
                    ID_FILIAL = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_TB_USUARIO_TB_FILIAL_ID_FILIAL",
                        column: x => x.ID_FILIAL,
                        principalTable: "TB_FILIAL",
                        principalColumn: "ID_FILIAL",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_AGENDAMENTO",
                columns: table => new
                {
                    ID_AGENDAMENTO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_MOTO = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    DT_AGENDADA = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    DS_DESCRICAO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    DT_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, defaultValueSql: "SYSTIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AGENDAMENTO", x => x.ID_AGENDAMENTO);
                    table.ForeignKey(
                        name: "FK_TB_AGENDAMENTO_TB_MOTO_ID_MOTO",
                        column: x => x.ID_MOTO,
                        principalTable: "TB_MOTO",
                        principalColumn: "ID_MOTO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_EVENTO",
                columns: table => new
                {
                    ID_EVENTO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_MOTO = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    TP_EVENTO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DS_MOTIVO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    DT_HR_EVENTO = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "SYSTIMESTAMP"),
                    DS_LOCALIZACAO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_EVENTO", x => x.ID_EVENTO);
                    table.ForeignKey(
                        name: "FK_TB_EVENTO_TB_MOTO_ID_MOTO",
                        column: x => x.ID_MOTO,
                        principalTable: "TB_MOTO",
                        principalColumn: "ID_MOTO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AGENDAMENTO_MOTO",
                table: "TB_AGENDAMENTO",
                column: "ID_MOTO");

            migrationBuilder.CreateIndex(
                name: "IX_EVENTO_MOTO",
                table: "TB_EVENTO",
                column: "ID_MOTO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MOTO_ID_FILIAL",
                table: "TB_MOTO",
                column: "ID_FILIAL");

            migrationBuilder.CreateIndex(
                name: "UX_MOTO_PLACA",
                table: "TB_MOTO",
                column: "CD_PLACA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_ID_FILIAL",
                table: "TB_USUARIO",
                column: "ID_FILIAL");

            migrationBuilder.CreateIndex(
                name: "UX_USUARIO_EMAIL",
                table: "TB_USUARIO",
                column: "DS_EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_AGENDAMENTO");

            migrationBuilder.DropTable(
                name: "TB_EVENTO");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_MOTO");

            migrationBuilder.DropTable(
                name: "TB_FILIAL");
        }
    }
}
