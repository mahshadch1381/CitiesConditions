using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBFIRST_Cities3.Migrations
{
    /// <inheritdoc />
    public partial class YourMigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompeleteName1");

            migrationBuilder.DropTable(
                name: "Populations");

            migrationBuilder.DropColumn(
                name: "modifytime",
                table: "Cities");

            migrationBuilder.AddColumn<string>(
                name: "modifitime",
                table: "Cities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "compeleNameofcity",
                columns: table => new
                {
                    CompID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CopmName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compeleNameofcity", x => x.CompID);
                });

            migrationBuilder.CreateTable(
                name: "pop",
                columns: table => new
                {
                    popID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Popvalue = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pop", x => x.popID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_compeleteNameID",
                table: "Cities",
                column: "compeleteNameID");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_countryId",
                table: "Cities",
                column: "countryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_humidityID",
                table: "Cities",
                column: "humidityID");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_LongId",
                table: "Cities",
                column: "LongId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_populationID",
                table: "Cities",
                column: "populationID");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_TempID",
                table: "Cities",
                column: "TempID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_COUNTRY1",
                table: "Cities",
                column: "countryId",
                principalTable: "COUNTRY1",
                principalColumn: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Cities",
                table: "Cities",
                column: "compeleteNameID",
                principalTable: "compeleNameofcity",
                principalColumn: "CompID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Humiditys",
                table: "Cities",
                column: "humidityID",
                principalTable: "Humiditys",
                principalColumn: "Humidityid");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Longitudes",
                table: "Cities",
                column: "LongId",
                principalTable: "Longitudes",
                principalColumn: "LongitudeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Temperatures",
                table: "Cities",
                column: "TempID",
                principalTable: "Temperatures",
                principalColumn: "TempID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_pop",
                table: "Cities",
                column: "populationID",
                principalTable: "pop",
                principalColumn: "popID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_COUNTRY1",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Cities",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Humiditys",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Longitudes",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Temperatures",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_pop",
                table: "Cities");

            migrationBuilder.DropTable(
                name: "compeleNameofcity");

            migrationBuilder.DropTable(
                name: "pop");

            migrationBuilder.DropIndex(
                name: "IX_Cities_compeleteNameID",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_countryId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_humidityID",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_LongId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_populationID",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_TempID",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "modifitime",
                table: "Cities");

            migrationBuilder.AddColumn<DateTime>(
                name: "modifytime",
                table: "Cities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "CompeleteName1",
                columns: table => new
                {
                    CompeleteNameID = table.Column<int>(type: "int", nullable: true),
                    CompeleteName_Name = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Populations",
                columns: table => new
                {
                    Population = table.Column<int>(type: "int", nullable: true),
                    PopulationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });
        }
    }
}
