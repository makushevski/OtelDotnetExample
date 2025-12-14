using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OtelDotnetExample.Api.DataLayer.DbModels;

namespace OtelDotnetExample.Api.DataLayer.Migrations;

[DbContext(typeof(PgContext))]
[Migration("202512080001_InitDb")]
public class InitDbMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "weather_data",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                temperature = table.Column<int>(type: "integer", nullable: false),
                humidity = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table => { table.PrimaryKey("PK_weather_data", x => x.id); });

    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("weather_data");
    }
}
