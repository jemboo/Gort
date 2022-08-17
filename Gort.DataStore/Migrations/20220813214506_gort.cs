using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gort.DataStore.Migrations
{
    public partial class gort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BitPackR",
                columns: table => new
                {
                    BitPackRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BitsPerSymbol = table.Column<int>(type: "int", nullable: false),
                    SymbolCount = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitPackR", x => x.BitPackRId);
                })
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
                name: "CauseR",
                columns: table => new
                {
                    CauseRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comments = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CauseStatus = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauseR", x => x.CauseRId);
                    table.ForeignKey(
                        name: "FK_CauseR_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "WorkspaceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CauseParamR",
                columns: table => new
                {
                    CauseParamRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseRId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauseParamR", x => x.CauseParamRId);
                    table.ForeignKey(
                        name: "FK_CauseParamR_CauseR_CauseRId",
                        column: x => x.CauseRId,
                        principalTable: "CauseR",
                        principalColumn: "CauseRId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CauseParamR_Param_ParamId",
                        column: x => x.ParamId,
                        principalTable: "Param",
                        principalColumn: "ParamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RandGenR",
                columns: table => new
                {
                    RandGenRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseId = table.Column<int>(type: "int", nullable: false),
                    CauseRId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Json = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandGenR", x => x.RandGenRId);
                    table.ForeignKey(
                        name: "FK_RandGenR_CauseR_CauseRId",
                        column: x => x.CauseRId,
                        principalTable: "CauseR",
                        principalColumn: "CauseRId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SortableSetR",
                columns: table => new
                {
                    SortableSetRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseRId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Json = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BitPackRId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortableSetR", x => x.SortableSetRId);
                    table.ForeignKey(
                        name: "FK_SortableSetR_BitPackR_BitPackRId",
                        column: x => x.BitPackRId,
                        principalTable: "BitPackR",
                        principalColumn: "BitPackRId");
                    table.ForeignKey(
                        name: "FK_SortableSetR_CauseR_CauseRId",
                        column: x => x.CauseRId,
                        principalTable: "CauseR",
                        principalColumn: "CauseRId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterR",
                columns: table => new
                {
                    SorterRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseRId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Json = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BitPackRId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterR", x => x.SorterRId);
                    table.ForeignKey(
                        name: "FK_SorterR_BitPackR_BitPackRId",
                        column: x => x.BitPackRId,
                        principalTable: "BitPackR",
                        principalColumn: "BitPackRId");
                    table.ForeignKey(
                        name: "FK_SorterR_CauseR_CauseRId",
                        column: x => x.CauseRId,
                        principalTable: "CauseR",
                        principalColumn: "CauseRId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterSetR",
                columns: table => new
                {
                    SorterSetRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseRId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Json = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BitPackRId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterSetR", x => x.SorterSetRId);
                    table.ForeignKey(
                        name: "FK_SorterSetR_BitPackR_BitPackRId",
                        column: x => x.BitPackRId,
                        principalTable: "BitPackR",
                        principalColumn: "BitPackRId");
                    table.ForeignKey(
                        name: "FK_SorterSetR_CauseR_CauseRId",
                        column: x => x.CauseRId,
                        principalTable: "CauseR",
                        principalColumn: "CauseRId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SorterSetPerfR",
                columns: table => new
                {
                    SorterSetPerfRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CauseRId = table.Column<int>(type: "int", nullable: false),
                    CausePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SorterSetRId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Json = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BitPackRId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SorterSetPerfR", x => x.SorterSetPerfRId);
                    table.ForeignKey(
                        name: "FK_SorterSetPerfR_BitPackR_BitPackRId",
                        column: x => x.BitPackRId,
                        principalTable: "BitPackR",
                        principalColumn: "BitPackRId");
                    table.ForeignKey(
                        name: "FK_SorterSetPerfR_CauseR_CauseRId",
                        column: x => x.CauseRId,
                        principalTable: "CauseR",
                        principalColumn: "CauseRId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SorterSetPerfR_SorterSetR_SorterSetRId",
                        column: x => x.SorterSetRId,
                        principalTable: "SorterSetR",
                        principalColumn: "SorterSetRId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CauseParamR_CauseRId",
                table: "CauseParamR",
                column: "CauseRId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseParamR_ParamId",
                table: "CauseParamR",
                column: "ParamId");

            migrationBuilder.CreateIndex(
                name: "IX_CauseR_WorkspaceId_Index",
                table: "CauseR",
                columns: new[] { "WorkspaceId", "Index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RandGenR_CauseRId",
                table: "RandGenR",
                column: "CauseRId");

            migrationBuilder.CreateIndex(
                name: "IX_SortableSetR_BitPackRId",
                table: "SortableSetR",
                column: "BitPackRId");

            migrationBuilder.CreateIndex(
                name: "IX_SortableSetR_CauseRId",
                table: "SortableSetR",
                column: "CauseRId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterR_BitPackRId",
                table: "SorterR",
                column: "BitPackRId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterR_CauseRId",
                table: "SorterR",
                column: "CauseRId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetPerfR_BitPackRId",
                table: "SorterSetPerfR",
                column: "BitPackRId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetPerfR_CauseRId",
                table: "SorterSetPerfR",
                column: "CauseRId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetPerfR_SorterSetRId",
                table: "SorterSetPerfR",
                column: "SorterSetRId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetR_BitPackRId",
                table: "SorterSetR",
                column: "BitPackRId");

            migrationBuilder.CreateIndex(
                name: "IX_SorterSetR_CauseRId",
                table: "SorterSetR",
                column: "CauseRId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CauseParamR");

            migrationBuilder.DropTable(
                name: "RandGenR");

            migrationBuilder.DropTable(
                name: "SortableSetR");

            migrationBuilder.DropTable(
                name: "SorterR");

            migrationBuilder.DropTable(
                name: "SorterSetPerfR");

            migrationBuilder.DropTable(
                name: "Param");

            migrationBuilder.DropTable(
                name: "SorterSetR");

            migrationBuilder.DropTable(
                name: "BitPackR");

            migrationBuilder.DropTable(
                name: "CauseR");

            migrationBuilder.DropTable(
                name: "Workspace");
        }
    }
}
