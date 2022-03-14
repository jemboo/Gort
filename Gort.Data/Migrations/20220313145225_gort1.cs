using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gort.Data.Migrations
{
    public partial class gort1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CauseTypeGroup",
                columns: table => new
                {
                    CauseTypeGroupId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauseTypeGroup", x => x.CauseTypeGroupId);
                    table.ForeignKey(
                        name: "FK_CauseTypeGroup_CauseTypeGroup_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CauseTypeGroup",
                        principalColumn: "CauseTypeGroupId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Workspace",
                columns: table => new
                {
                    WorkspaceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspace", x => x.WorkspaceId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CauseType",
                columns: table => new
                {
                    CauseTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseTypeGroupId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauseType", x => x.CauseTypeId);
                    table.ForeignKey(
                        name: "FK_CauseType_CauseTypeGroup_CauseTypeGroupId",
                        column: x => x.CauseTypeGroupId,
                        principalTable: "CauseTypeGroup",
                        principalColumn: "CauseTypeGroupId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cause",
                columns: table => new
                {
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseTypeID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseStatus = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cause", x => x.CauseId);
                    table.ForeignKey(
                        name: "FK_Cause_CauseType_CauseTypeID",
                        column: x => x.CauseTypeID,
                        principalTable: "CauseType",
                        principalColumn: "CauseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cause_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "WorkspaceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CauseTypeParam",
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
                    table.PrimaryKey("PK_CauseTypeParam", x => x.CauseTypeParamId);
                    table.ForeignKey(
                        name: "FK_CauseTypeParam_CauseType_CauseTypeId",
                        column: x => x.CauseTypeId,
                        principalTable: "CauseType",
                        principalColumn: "CauseTypeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RandGen",
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
                    table.PrimaryKey("PK_RandGen", x => x.RandGenId);
                    table.ForeignKey(
                        name: "FK_RandGen_Cause_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Cause",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sortable",
                columns: table => new
                {
                    SortableId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StructId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SortableSetId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    SortableFormat = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sortable", x => x.SortableId);
                    table.ForeignKey(
                        name: "FK_Sortable_Cause_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Cause",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SortableSet",
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
                    SortableFormat = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortableSet", x => x.SortableSetId);
                    table.ForeignKey(
                        name: "FK_SortableSet_Cause_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Cause",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sorter",
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
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorter", x => x.SorterId);
                    table.ForeignKey(
                        name: "FK_Sorter_Cause_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Cause",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterSet",
                columns: table => new
                {
                    SorterSetId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    SwitchLength = table.Column<int>(type: "int", nullable: false),
                    IsGenerated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterSet", x => x.SorterSetId);
                    table.ForeignKey(
                        name: "FK_SorterSet_Cause_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Cause",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CauseParam",
                columns: table => new
                {
                    CauseParamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CauseTypeParamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Value = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauseParam", x => x.CauseParamId);
                    table.ForeignKey(
                        name: "FK_CauseParam_Cause_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Cause",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CauseParam_CauseTypeParam_CauseTypeParamId",
                        column: x => x.CauseTypeParamId,
                        principalTable: "CauseTypeParam",
                        principalColumn: "CauseTypeParamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterPerf",
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
                    NumberFormat = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterPerf", x => x.SorterPerfId);
                    table.ForeignKey(
                        name: "FK_SorterPerf_Cause_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Cause",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SorterPerf_SortableSet_SortableSetId",
                        column: x => x.SortableSetId,
                        principalTable: "SortableSet",
                        principalColumn: "SortableSetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SorterPerf_Sorter_SorterId",
                        column: x => x.SorterId,
                        principalTable: "Sorter",
                        principalColumn: "SorterId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterSetPerf",
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
                    NumberFormat = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterSetPerf", x => x.SorterSetPerfId);
                    table.ForeignKey(
                        name: "FK_SorterSetPerf_Cause_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Cause",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SorterSetPerf_SortableSet_SortableSetId",
                        column: x => x.SortableSetId,
                        principalTable: "SortableSet",
                        principalColumn: "SortableSetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SorterSetPerf_SorterSet_SorterSetId",
                        column: x => x.SorterSetId,
                        principalTable: "SorterSet",
                        principalColumn: "SorterSetId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Cause_CauseTypeID",
                table: "Cause",
                column: "CauseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Cause_WorkspaceId",
                table: "Cause",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseParam_CauseId",
                table: "CauseParam",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseParam_CauseTypeParamId",
                table: "CauseParam",
                column: "CauseTypeParamId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseType_CauseTypeGroupId",
                table: "CauseType",
                column: "CauseTypeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseTypeGroup_ParentId",
                table: "CauseTypeGroup",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseTypeParam_CauseTypeId",
                table: "CauseTypeParam",
                column: "CauseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RandGen_CauseId",
                table: "RandGen",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sortable_CauseId",
                table: "Sortable",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SortableSet_CauseId",
                table: "SortableSet",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorter_CauseId",
                table: "Sorter",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterPerf_CauseId",
                table: "SorterPerf",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterPerf_SortableSetId",
                table: "SorterPerf",
                column: "SortableSetId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterPerf_SorterId",
                table: "SorterPerf",
                column: "SorterId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSet_CauseId",
                table: "SorterSet",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetPerf_CauseId",
                table: "SorterSetPerf",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetPerf_SortableSetId",
                table: "SorterSetPerf",
                column: "SortableSetId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetPerf_SorterSetId",
                table: "SorterSetPerf",
                column: "SorterSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CauseParam");

            migrationBuilder.DropTable(
                name: "RandGen");

            migrationBuilder.DropTable(
                name: "Sortable");

            migrationBuilder.DropTable(
                name: "SorterPerf");

            migrationBuilder.DropTable(
                name: "SorterSetPerf");

            migrationBuilder.DropTable(
                name: "CauseTypeParam");

            migrationBuilder.DropTable(
                name: "Sorter");

            migrationBuilder.DropTable(
                name: "SortableSet");

            migrationBuilder.DropTable(
                name: "SorterSet");

            migrationBuilder.DropTable(
                name: "Cause");

            migrationBuilder.DropTable(
                name: "CauseType");

            migrationBuilder.DropTable(
                name: "Workspace");

            migrationBuilder.DropTable(
                name: "CauseTypeGroup");
        }
    }
}
