using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gort.DataStore.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Param",
                columns: table => new
                {
                    ParamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParamDataType = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<byte[]>(type: "longblob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Param", x => x.ParamId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Workspace",
                columns: table => new
                {
                    WorkspaceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspace", x => x.WorkspaceId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cause",
                columns: table => new
                {
                    CauseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Genus = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Species = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comments = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseStatus = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cause", x => x.CauseId);
                    table.ForeignKey(
                        name: "FK_Cause_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "WorkspaceId",
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
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParamId = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_CauseParam_Param_ParamId",
                        column: x => x.ParamId,
                        principalTable: "Param",
                        principalColumn: "ParamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RandGen",
                columns: table => new
                {
                    RandGenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cereal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                    SortableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cereal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                name: "SortableGen",
                columns: table => new
                {
                    SortableGenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cereal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortableGen", x => x.SortableGenId);
                    table.ForeignKey(
                        name: "FK_SortableGen_Cause_CauseId",
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
                    SortableSetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cereal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                    SorterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cereal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                name: "SorterGen",
                columns: table => new
                {
                    SorterGenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cereal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterGen", x => x.SorterGenId);
                    table.ForeignKey(
                        name: "FK_SorterGen_Cause_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Cause",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterMutator",
                columns: table => new
                {
                    SorterMutatorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cereal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterMutator", x => x.SorterMutatorId);
                    table.ForeignKey(
                        name: "FK_SorterMutator_Cause_CauseId",
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
                    SorterSetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cereal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                name: "SorterPerf",
                columns: table => new
                {
                    SorterPerfId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cereal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false),
                    SorterSetId = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_SorterPerf_SorterSet_SorterSetId",
                        column: x => x.SorterSetId,
                        principalTable: "SorterSet",
                        principalColumn: "SorterSetId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterSetPerf",
                columns: table => new
                {
                    SorterSetPerfId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cereal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false),
                    SorterSetId = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_SorterSetPerf_SorterSet_SorterSetId",
                        column: x => x.SorterSetId,
                        principalTable: "SorterSet",
                        principalColumn: "SorterSetId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Cause_WorkspaceId_Index",
                table: "Cause",
                columns: new[] { "WorkspaceId", "Index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CauseParam_CauseId",
                table: "CauseParam",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseParam_ParamId",
                table: "CauseParam",
                column: "ParamId");

            migrationBuilder.CreateIndex(
                name: "IX_RandGen_CauseId",
                table: "RandGen",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sortable_CauseId",
                table: "Sortable",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SortableGen_CauseId",
                table: "SortableGen",
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
                name: "IX_SorterGen_CauseId",
                table: "SorterGen",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterMutator_CauseId",
                table: "SorterMutator",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterPerf_CauseId",
                table: "SorterPerf",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterPerf_SorterSetId",
                table: "SorterPerf",
                column: "SorterSetId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSet_CauseId",
                table: "SorterSet",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetPerf_CauseId",
                table: "SorterSetPerf",
                column: "CauseId");

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
                name: "SortableGen");

            migrationBuilder.DropTable(
                name: "SortableSet");

            migrationBuilder.DropTable(
                name: "Sorter");

            migrationBuilder.DropTable(
                name: "SorterGen");

            migrationBuilder.DropTable(
                name: "SorterMutator");

            migrationBuilder.DropTable(
                name: "SorterPerf");

            migrationBuilder.DropTable(
                name: "SorterSetPerf");

            migrationBuilder.DropTable(
                name: "Param");

            migrationBuilder.DropTable(
                name: "SorterSet");

            migrationBuilder.DropTable(
                name: "Cause");

            migrationBuilder.DropTable(
                name: "Workspace");
        }
    }
}
