using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaApiOracle.Migrations
{
    /// <inheritdoc />
    public partial class NovaEstruturaBancoDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    ID_CURSO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME_CURSO = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QT_HORAS = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.ID_CURSO);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    ID_EMPRESA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAZAO_SOCIAL = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CNPJ = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    EMAIL_EMPRESA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.ID_EMPRESA);
                });

            migrationBuilder.CreateTable(
                name: "LogAuditorias",
                columns: table => new
                {
                    ID_LOG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME_TABELA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DS_OPERACAO = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DATA_OPERACAO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NM_USUARIO_DB = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAuditorias", x => x.ID_LOG);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EMAIL_USUARIO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SENHA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CADASTRO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.ID_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "Vagas",
                columns: table => new
                {
                    ID_VAGA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME_VAGA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DESCRICAO_VAGA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SALARIO = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    DT_PUBLICACAO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ID_EMPRESA = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vagas", x => x.ID_VAGA);
                    table.ForeignKey(
                        name: "FK_Vagas_Empresas_ID_EMPRESA",
                        column: x => x.ID_EMPRESA,
                        principalTable: "Empresas",
                        principalColumn: "ID_EMPRESA",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Certificados",
                columns: table => new
                {
                    ID_CERTIFICADO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DT_EMISSAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CODIGO_VALIDACAO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ID_USUARIO = table.Column<int>(type: "int", nullable: false),
                    ID_CURSO = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificados", x => x.ID_CERTIFICADO);
                    table.ForeignKey(
                        name: "FK_Certificados_Cursos_ID_CURSO",
                        column: x => x.ID_CURSO,
                        principalTable: "Cursos",
                        principalColumn: "ID_CURSO",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Certificados_Usuarios_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "Usuarios",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificados_ID_CURSO",
                table: "Certificados",
                column: "ID_CURSO");

            migrationBuilder.CreateIndex(
                name: "IX_Certificados_ID_USUARIO",
                table: "Certificados",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_CNPJ",
                table: "Empresas",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CPF",
                table: "Usuarios",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_ID_EMPRESA",
                table: "Vagas",
                column: "ID_EMPRESA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificados");

            migrationBuilder.DropTable(
                name: "LogAuditorias");

            migrationBuilder.DropTable(
                name: "Vagas");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
