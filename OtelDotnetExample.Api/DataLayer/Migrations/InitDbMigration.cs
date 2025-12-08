using Microsoft.EntityFrameworkCore.Migrations;

namespace OtelDotnetExample.Api.DataLayer.Migrations;

public class InitDbMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable("weather-data", builder =>
        {
            return new[]
            {
                builder.Column<Guid>(name: "id", nullable: false),
                builder.Column<int>(name: "temperature", nullable: false),
                builder.Column<int>(name: "humidity", nullable: false),
            };
        });
        
        migrationBuilder.InsertData("weather-data", ["id", "temperature", "humidity"], [Guid.NewGuid(), 20, 80]);
    }
}