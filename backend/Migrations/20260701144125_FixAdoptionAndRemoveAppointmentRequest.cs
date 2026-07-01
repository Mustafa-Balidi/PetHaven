using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHaven.Migrations
{
    /// <inheritdoc />
    public partial class FixAdoptionAndRemoveAppointmentRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetReports_Adopters_AdoptionRequestId",
                table: "PetReports");

            migrationBuilder.DropTable(
                name: "AppointmentRequests");

            migrationBuilder.AddColumn<int>(
                name: "AdopterId",
                table: "PetReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdoptionRequests",
                columns: table => new
                {
                    AdoptionRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdopterId = table.Column<int>(type: "int", nullable: false),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CenterNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdoptionRequests", x => x.AdoptionRequestId);
                    table.ForeignKey(
                        name: "FK_AdoptionRequests_Adopters_AdopterId",
                        column: x => x.AdopterId,
                        principalTable: "Adopters",
                        principalColumn: "AdopterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdoptionRequests_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetReports_AdopterId",
                table: "PetReports",
                column: "AdopterId");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionRequests_AdopterId",
                table: "AdoptionRequests",
                column: "AdopterId");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionRequests_PetId",
                table: "AdoptionRequests",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetReports_Adopters_AdopterId",
                table: "PetReports",
                column: "AdopterId",
                principalTable: "Adopters",
                principalColumn: "AdopterId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetReports_AdoptionRequests_AdoptionRequestId",
                table: "PetReports",
                column: "AdoptionRequestId",
                principalTable: "AdoptionRequests",
                principalColumn: "AdoptionRequestId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetReports_Adopters_AdopterId",
                table: "PetReports");

            migrationBuilder.DropForeignKey(
                name: "FK_PetReports_AdoptionRequests_AdoptionRequestId",
                table: "PetReports");

            migrationBuilder.DropTable(
                name: "AdoptionRequests");

            migrationBuilder.DropIndex(
                name: "IX_PetReports_AdopterId",
                table: "PetReports");

            migrationBuilder.DropColumn(
                name: "AdopterId",
                table: "PetReports");

            migrationBuilder.CreateTable(
                name: "AppointmentRequests",
                columns: table => new
                {
                    AppointmentRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HealthStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentRequests", x => x.AppointmentRequestId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PetReports_Adopters_AdoptionRequestId",
                table: "PetReports",
                column: "AdoptionRequestId",
                principalTable: "Adopters",
                principalColumn: "AdopterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
