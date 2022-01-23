using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gort.Data.Migrations
{
    public partial class start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CauseTypeGroups",
                columns: table => new
                {
                    CauseTypeGroupId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
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
                name: "RndGens",
                columns: table => new
                {
                    RndGenId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Seed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RndGens", x => x.RndGenId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SortableSets",
                columns: table => new
                {
                    WorkspaceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortableSets", x => x.WorkspaceId);
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
                name: "CauseTypes",
                columns: table => new
                {
                    CauseTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseTypeGroupId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauseTypes", x => x.CauseTypeId);
                    table.ForeignKey(
                        name: "FK_CauseTypes_CauseTypeGroups_CauseTypeGroupId",
                        column: x => x.CauseTypeGroupId,
                        principalTable: "CauseTypeGroups",
                        principalColumn: "CauseTypeGroupId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PerfBinSets",
                columns: table => new
                {
                    CauseTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseTypeGroupId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfBinSets", x => x.CauseTypeId);
                    table.ForeignKey(
                        name: "FK_PerfBinSets_CauseTypeGroups_CauseTypeGroupId",
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
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SortableSetWorkspaceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Causes", x => x.CauseId);
                    table.ForeignKey(
                        name: "FK_Causes_CauseTypes_CauseTypeId",
                        column: x => x.CauseTypeId,
                        principalTable: "CauseTypes",
                        principalColumn: "CauseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Causes_SortableSets_SortableSetWorkspaceId",
                        column: x => x.SortableSetWorkspaceId,
                        principalTable: "SortableSets",
                        principalColumn: "WorkspaceId");
                    table.ForeignKey(
                        name: "FK_Causes_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspaces",
                        principalColumn: "WorkspaceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CauseTypeParams",
                columns: table => new
                {
                    CauseTypeParamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauseTypeParams", x => x.CauseTypeParamId);
                    table.ForeignKey(
                        name: "FK_CauseTypeParams_CauseTypes_CauseTypeId",
                        column: x => x.CauseTypeId,
                        principalTable: "CauseTypes",
                        principalColumn: "CauseTypeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sorters",
                columns: table => new
                {
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorters", x => x.CauseId);
                    table.ForeignKey(
                        name: "FK_Sorters_CauseTypes_CauseTypeId",
                        column: x => x.CauseTypeId,
                        principalTable: "CauseTypes",
                        principalColumn: "CauseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sorters_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspaces",
                        principalColumn: "WorkspaceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterSets",
                columns: table => new
                {
                    CauseParamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseTypeParamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterSets", x => x.CauseParamId);
                    table.ForeignKey(
                        name: "FK_SorterSets_Causes_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Causes",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SorterSets_CauseTypeParams_CauseTypeParamId",
                        column: x => x.CauseTypeParamId,
                        principalTable: "CauseTypeParams",
                        principalColumn: "CauseTypeParamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CauseParams",
                columns: table => new
                {
                    CauseParamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseTypeParamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SorterCauseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauseParams", x => x.CauseParamId);
                    table.ForeignKey(
                        name: "FK_CauseParams_Causes_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Causes",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CauseParams_CauseTypeParams_CauseTypeParamId",
                        column: x => x.CauseTypeParamId,
                        principalTable: "CauseTypeParams",
                        principalColumn: "CauseTypeParamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CauseParams_Sorters_SorterCauseId",
                        column: x => x.SorterCauseId,
                        principalTable: "Sorters",
                        principalColumn: "CauseId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CauseParams_CauseId",
                table: "CauseParams",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseParams_CauseTypeParamId",
                table: "CauseParams",
                column: "CauseTypeParamId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseParams_SorterCauseId",
                table: "CauseParams",
                column: "SorterCauseId");

            migrationBuilder.CreateIndex(
                name: "IX_Causes_CauseTypeId",
                table: "Causes",
                column: "CauseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Causes_SortableSetWorkspaceId",
                table: "Causes",
                column: "SortableSetWorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Causes_WorkspaceId",
                table: "Causes",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseTypeGroups_ParentId",
                table: "CauseTypeGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseTypeParams_CauseTypeId",
                table: "CauseTypeParams",
                column: "CauseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseTypes_CauseTypeGroupId",
                table: "CauseTypes",
                column: "CauseTypeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfBinSets_CauseTypeGroupId",
                table: "PerfBinSets",
                column: "CauseTypeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorters_CauseTypeId",
                table: "Sorters",
                column: "CauseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorters_WorkspaceId",
                table: "Sorters",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSets_CauseId",
                table: "SorterSets",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSets_CauseTypeParamId",
                table: "SorterSets",
                column: "CauseTypeParamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CauseParams");

            migrationBuilder.DropTable(
                name: "PerfBinSets");

            migrationBuilder.DropTable(
                name: "RndGens");

            migrationBuilder.DropTable(
                name: "SorterSets");

            migrationBuilder.DropTable(
                name: "Sorters");

            migrationBuilder.DropTable(
                name: "Causes");

            migrationBuilder.DropTable(
                name: "CauseTypeParams");

            migrationBuilder.DropTable(
                name: "SortableSets");

            migrationBuilder.DropTable(
                name: "Workspaces");

            migrationBuilder.DropTable(
                name: "CauseTypes");

            migrationBuilder.DropTable(
                name: "CauseTypeGroups");
        }
    }
}
