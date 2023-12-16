using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolForecast.Api.Migrations
{
    /// <inheritdoc />
    public partial class Trainings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataOid = table.Column<uint>(type: "oid", nullable: false),
                    Accuracy = table.Column<double>(type: "double precision", nullable: false),
                    Recall = table.Column<double>(type: "double precision", nullable: false),
                    F1Score = table.Column<double>(type: "double precision", nullable: false),
                    MeanSquaredError = table.Column<double>(type: "double precision", nullable: false),
                    R2 = table.Column<double>(type: "double precision", nullable: false),
                    AucRoc = table.Column<double>(type: "double precision", nullable: false),
                    LogLoss = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trainings");
        }
    }
}
