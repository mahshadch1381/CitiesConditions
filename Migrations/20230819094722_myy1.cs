using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBFIRST_Cities3.Migrations
{
    /// <inheritdoc />
    public partial class myy1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "COUNTRY1",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COUNTRY1", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Humiditys",
                columns: table => new
                {
                    Humidityid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Humidity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Humiditys", x => x.Humidityid);
                });

            migrationBuilder.CreateTable(
                name: "Latitudes",
                columns: table => new
                {
                    LatitudeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Latitudes", x => x.LatitudeId);
                });

            migrationBuilder.CreateTable(
                name: "Longitudes",
                columns: table => new
                {
                    LongitudeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Longitude = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Longitudes", x => x.LongitudeId);
                });

            migrationBuilder.CreateTable(
                name: "Populations",
                columns: table => new
                {
                    PopulationId = table.Column<int>(type: "int", nullable: true),
                    Population = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Temperatures",
                columns: table => new
                {
                    TempID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Temperature = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temperatures", x => x.TempID);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    humidityID = table.Column<int>(type: "int", nullable: true),
                    TempID = table.Column<int>(type: "int", nullable: true),
                    countryId = table.Column<int>(type: "int", nullable: true),
                    compeleteNameID = table.Column<int>(type: "int", nullable: true),
                    populationID = table.Column<int>(type: "int", nullable: true),
                    LongId = table.Column<int>(type: "int", nullable: true),
                    LatId = table.Column<int>(type: "int", nullable: true),
                    modifytime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Cities_Latitudes",
                        column: x => x.LatId,
                        principalTable: "Latitudes",
                        principalColumn: "LatitudeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_LatId",
                table: "Cities",
                column: "LatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "CompeleteName1");

            migrationBuilder.DropTable(
                name: "COUNTRY1");

            migrationBuilder.DropTable(
                name: "Humiditys");

            migrationBuilder.DropTable(
                name: "Longitudes");

            migrationBuilder.DropTable(
                name: "Populations");

            migrationBuilder.DropTable(
                name: "Temperatures");

            migrationBuilder.DropTable(
                name: "Latitudes");
        }
    }
}
