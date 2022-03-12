using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gort.Data.Migrations
{
    public partial class gort : Migration
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
                name: "Causes",
                columns: table => new
                {
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseStatus = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                    DataType = table.Column<int>(type: "int", nullable: false)
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
                name: "RandGens",
                columns: table => new
                {
                    RandGenId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StructId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Seed = table.Column<int>(type: "int", nullable: false),
                    RndGenType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandGens", x => x.RandGenId);
                    table.ForeignKey(
                        name: "FK_RandGens_Causes_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Causes",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SortableSets",
                columns: table => new
                {
                    SortableSetId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StructId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SortableSetRep = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    BinaryFormat = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortableSets", x => x.SortableSetId);
                    table.ForeignKey(
                        name: "FK_SortableSets_Causes_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Causes",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sorters",
                columns: table => new
                {
                    SorterId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StructId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    BinaryFormat = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorters", x => x.SorterId);
                    table.ForeignKey(
                        name: "FK_Sorters_Causes_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Causes",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterSets",
                columns: table => new
                {
                    SorterSetId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    SorterSetRep = table.Column<int>(type: "int", nullable: false),
                    BinaryFormat = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterSets", x => x.SorterSetId);
                    table.ForeignKey(
                        name: "FK_SorterSets_Causes_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Causes",
                        principalColumn: "CauseId",
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
                    Value = table.Column<byte[]>(type: "longblob", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterPerfs",
                columns: table => new
                {
                    SorterPerfId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SorterId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SortableSetId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SorterPerfRep = table.Column<int>(type: "int", nullable: false),
                    BinaryFormat = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterPerfs", x => x.SorterPerfId);
                    table.ForeignKey(
                        name: "FK_SorterPerfs_Causes_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Causes",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SorterPerfs_SortableSets_SortableSetId",
                        column: x => x.SortableSetId,
                        principalTable: "SortableSets",
                        principalColumn: "SortableSetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SorterPerfs_Sorters_SorterId",
                        column: x => x.SorterId,
                        principalTable: "Sorters",
                        principalColumn: "SorterId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterSetPerfs",
                columns: table => new
                {
                    SorterSetPerfId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SorterSetId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SortableSetId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SorterSetPerfRep = table.Column<int>(type: "int", nullable: false),
                    SorterSetPerfData = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterSetPerfs", x => x.SorterSetPerfId);
                    table.ForeignKey(
                        name: "FK_SorterSetPerfs_Causes_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Causes",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SorterSetPerfs_SortableSets_SortableSetId",
                        column: x => x.SortableSetId,
                        principalTable: "SortableSets",
                        principalColumn: "SortableSetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SorterSetPerfs_SorterSets_SorterSetId",
                        column: x => x.SorterSetId,
                        principalTable: "SorterSets",
                        principalColumn: "SorterSetId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_Causes_CauseTypeId",
                table: "Causes",
                column: "CauseTypeId");

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
                name: "IX_RandGens_CauseId",
                table: "RandGens",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SortableSets_CauseId",
                table: "SortableSets",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterPerfs_CauseId",
                table: "SorterPerfs",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterPerfs_SortableSetId",
                table: "SorterPerfs",
                column: "SortableSetId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterPerfs_SorterId",
                table: "SorterPerfs",
                column: "SorterId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorters_CauseId",
                table: "Sorters",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetPerfs_CauseId",
                table: "SorterSetPerfs",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetPerfs_SortableSetId",
                table: "SorterSetPerfs",
                column: "SortableSetId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetPerfs_SorterSetId",
                table: "SorterSetPerfs",
                column: "SorterSetId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSets_CauseId",
                table: "SorterSets",
                column: "CauseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CauseParams");

            migrationBuilder.DropTable(
                name: "RandGens");

            migrationBuilder.DropTable(
                name: "SorterPerfs");

            migrationBuilder.DropTable(
                name: "SorterSetPerfs");

            migrationBuilder.DropTable(
                name: "CauseTypeParams");

            migrationBuilder.DropTable(
                name: "Sorters");

            migrationBuilder.DropTable(
                name: "SortableSets");

            migrationBuilder.DropTable(
                name: "SorterSets");

            migrationBuilder.DropTable(
                name: "Causes");

            migrationBuilder.DropTable(
                name: "CauseTypes");

            migrationBuilder.DropTable(
                name: "Workspaces");

            migrationBuilder.DropTable(
                name: "CauseTypeGroups");
        }
    }
}
