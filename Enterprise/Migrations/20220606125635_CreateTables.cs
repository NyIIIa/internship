using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enterprise.Migrations
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subdivisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subdivisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Headmasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PatronicName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DateBirth = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headmasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Headmasters_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inspectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PatronicName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DateBirth = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspectors_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubdivisionMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PatronicName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DateBirth = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubdivisionMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubdivisionMasters_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workman",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PatronicName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DateBirth = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    FullNameSubdivisionMaster = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workman", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workman_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Headmasters_SubdivisionId",
                table: "Headmasters",
                column: "SubdivisionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inspectors_SubdivisionId",
                table: "Inspectors",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubdivisionMasters_SubdivisionId",
                table: "SubdivisionMasters",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Workman_SubdivisionId",
                table: "Workman",
                column: "SubdivisionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Headmasters");

            migrationBuilder.DropTable(
                name: "Inspectors");

            migrationBuilder.DropTable(
                name: "SubdivisionMasters");

            migrationBuilder.DropTable(
                name: "Workman");

            migrationBuilder.DropTable(
                name: "Subdivisions");
        }
    }
}
