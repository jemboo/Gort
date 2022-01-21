using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gort.Data.Migrations
{
    public partial class Start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CauseTypeGroups",
                columns: table => new
                {
                    CauseTypeGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauseTypeGroups", x => x.CauseTypeGroupId);
                    table.ForeignKey(
                        name: "FK_CauseTypeGroups_CauseTypeGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CauseTypeGroups",
                        principalColumn: "CauseTypeGroupId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Workspaces",
                columns: table => new
                {
                    WorkspaceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspaces", x => x.WorkspaceId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CauseType",
                columns: table => new
                {
                    CauseTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseTypeGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauseType", x => x.CauseTypeId);
                    table.ForeignKey(
                        name: "FK_CauseType_CauseTypeGroups_CauseTypeGroupId",
                        column: x => x.CauseTypeGroupId,
                        principalTable: "CauseTypeGroups",
                        principalColumn: "CauseTypeGroupId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Causes",
                columns: table => new
                {
                    CauseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseTypeId = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Causes", x => x.CauseId);
                    table.ForeignKey(
                        name: "FK_Causes_CauseType_CauseTypeId",
                        column: x => x.CauseTypeId,
                        principalTable: "CauseType",
                        principalColumn: "CauseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Causes_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspaces",
                        principalColumn: "WorkspaceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NamedParam",
                columns: table => new
                {
                    NamedParamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseTypeId = table.Column<int>(type: "int", nullable: false),
                    DataType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamedParam", x => x.NamedParamId);
                    table.ForeignKey(
                        name: "FK_NamedParam_CauseType_CauseTypeId",
                        column: x => x.CauseTypeId,
                        principalTable: "CauseType",
                        principalColumn: "CauseTypeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CauseParam",
                columns: table => new
                {
                    CauseParamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    NamedParamId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauseParam", x => x.CauseParamId);
                    table.ForeignKey(
                        name: "FK_CauseParam_Causes_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Causes",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CauseParam_NamedParam_NamedParamId",
                        column: x => x.NamedParamId,
                        principalTable: "NamedParam",
                        principalColumn: "NamedParamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CauseParam_CauseId",
                table: "CauseParam",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseParam_NamedParamId",
                table: "CauseParam",
                column: "NamedParamId");

            migrationBuilder.CreateIndex(
                name: "IX_Causes_CauseTypeId",
                table: "Causes",
                column: "CauseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Causes_WorkspaceId",
                table: "Causes",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseType_CauseTypeGroupId",
                table: "CauseType",
                column: "CauseTypeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseTypeGroups_ParentId",
                table: "CauseTypeGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_NamedParam_CauseTypeId",
                table: "NamedParam",
                column: "CauseTypeId");


            /////////////////

            migrationBuilder.InsertData(
                table: "CauseTypeGroups",
                columns: new[] { "Name" },
                values: new object[] { "root" });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CauseParam");

            migrationBuilder.DropTable(
                name: "Causes");

            migrationBuilder.DropTable(
                name: "NamedParam");

            migrationBuilder.DropTable(
                name: "Workspaces");

            migrationBuilder.DropTable(
                name: "CauseType");

            migrationBuilder.DropTable(
                name: "CauseTypeGroups");
        }
    }
}
