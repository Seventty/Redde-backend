using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Redde.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyMetadataTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Companies",
                newName: "CommercialName");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EconomicActivityId",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GovernmentBranchId",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentSchemeId",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RNC",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CompanyCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EconomicActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EconomicActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GovernmentBranches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovernmentBranches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentSchemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSchemes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CategoryId",
                table: "Companies",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_EconomicActivityId",
                table: "Companies",
                column: "EconomicActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_GovernmentBranchId",
                table: "Companies",
                column: "GovernmentBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PaymentSchemeId",
                table: "Companies",
                column: "PaymentSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_StateId",
                table: "Companies",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_CompanyCategories_CategoryId",
                table: "Companies",
                column: "CategoryId",
                principalTable: "CompanyCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_CompanyStates_StateId",
                table: "Companies",
                column: "StateId",
                principalTable: "CompanyStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_EconomicActivities_EconomicActivityId",
                table: "Companies",
                column: "EconomicActivityId",
                principalTable: "EconomicActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_GovernmentBranches_GovernmentBranchId",
                table: "Companies",
                column: "GovernmentBranchId",
                principalTable: "GovernmentBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_PaymentSchemes_PaymentSchemeId",
                table: "Companies",
                column: "PaymentSchemeId",
                principalTable: "PaymentSchemes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_CompanyCategories_CategoryId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_CompanyStates_StateId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_EconomicActivities_EconomicActivityId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_GovernmentBranches_GovernmentBranchId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_PaymentSchemes_PaymentSchemeId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "CompanyCategories");

            migrationBuilder.DropTable(
                name: "CompanyStates");

            migrationBuilder.DropTable(
                name: "EconomicActivities");

            migrationBuilder.DropTable(
                name: "GovernmentBranches");

            migrationBuilder.DropTable(
                name: "PaymentSchemes");

            migrationBuilder.DropIndex(
                name: "IX_Companies_CategoryId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_EconomicActivityId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_GovernmentBranchId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_PaymentSchemeId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_StateId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EconomicActivityId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "GovernmentBranchId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PaymentSchemeId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "RNC",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "CommercialName",
                table: "Companies",
                newName: "Address");
        }
    }
}
