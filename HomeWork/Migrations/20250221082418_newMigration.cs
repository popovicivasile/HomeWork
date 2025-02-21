using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeWork.Migrations
{
    /// <inheritdoc />
    public partial class newMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDentalProcedure_Doctors_DoctorId",
                table: "DoctorDentalProcedure");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDentalProcedure_RefDentalProcedures_RefDentalProcedureId",
                table: "DoctorDentalProcedure");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorDentalProcedure",
                table: "DoctorDentalProcedure");

            migrationBuilder.DropIndex(
                name: "IX_DoctorDentalProcedure_DoctorId",
                table: "DoctorDentalProcedure");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "DoctorDentalProcedure");

            migrationBuilder.RenameTable(
                name: "DoctorDentalProcedure",
                newName: "DoctorDentalProcedures");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorDentalProcedure_RefDentalProcedureId",
                table: "DoctorDentalProcedures",
                newName: "IX_DoctorDentalProcedures_RefDentalProcedureId");

            migrationBuilder.AddColumn<Guid>(
                name: "RefDentalProceduresId",
                table: "DoctorDentalProcedures",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DoctorDentalProcedures",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorDentalProcedures",
                table: "DoctorDentalProcedures",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "937de724-6de5-41c8-9187-a155079c94f8", "AQAAAAIAAYagAAAAEGPrngkGQK7hgJJFn2Cdw6M5RJS2UFpTPqzhI4QGzE+phbhvnzZvBipPfI7huUUbaw==" });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDentalProcedures_RefDentalProceduresId",
                table: "DoctorDentalProcedures",
                column: "RefDentalProceduresId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDentalProcedures_UserId",
                table: "DoctorDentalProcedures",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDentalProcedures_AspNetUsers_UserId",
                table: "DoctorDentalProcedures",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDentalProcedures_RefDentalProcedures_RefDentalProcedureId",
                table: "DoctorDentalProcedures",
                column: "RefDentalProcedureId",
                principalTable: "RefDentalProcedures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDentalProcedures_RefDentalProcedures_RefDentalProceduresId",
                table: "DoctorDentalProcedures",
                column: "RefDentalProceduresId",
                principalTable: "RefDentalProcedures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDentalProcedures_AspNetUsers_UserId",
                table: "DoctorDentalProcedures");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDentalProcedures_RefDentalProcedures_RefDentalProcedureId",
                table: "DoctorDentalProcedures");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDentalProcedures_RefDentalProcedures_RefDentalProceduresId",
                table: "DoctorDentalProcedures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorDentalProcedures",
                table: "DoctorDentalProcedures");

            migrationBuilder.DropIndex(
                name: "IX_DoctorDentalProcedures_RefDentalProceduresId",
                table: "DoctorDentalProcedures");

            migrationBuilder.DropIndex(
                name: "IX_DoctorDentalProcedures_UserId",
                table: "DoctorDentalProcedures");

            migrationBuilder.DropColumn(
                name: "RefDentalProceduresId",
                table: "DoctorDentalProcedures");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DoctorDentalProcedures");

            migrationBuilder.RenameTable(
                name: "DoctorDentalProcedures",
                newName: "DoctorDentalProcedure");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorDentalProcedures_RefDentalProcedureId",
                table: "DoctorDentalProcedure",
                newName: "IX_DoctorDentalProcedure_RefDentalProcedureId");

            migrationBuilder.AddColumn<Guid>(
                name: "DoctorId",
                table: "DoctorDentalProcedure",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorDentalProcedure",
                table: "DoctorDentalProcedure",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "acaf9310-3eda-4f94-9a3e-c877102b43f6", "AQAAAAIAAYagAAAAEIcEZqectFmbechnhvwlyPMAmm1c99PQYSCiXjupumeKd/BkNrydB5tJ1fwPGQfaRw==" });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDentalProcedure_DoctorId",
                table: "DoctorDentalProcedure",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDentalProcedure_Doctors_DoctorId",
                table: "DoctorDentalProcedure",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDentalProcedure_RefDentalProcedures_RefDentalProcedureId",
                table: "DoctorDentalProcedure",
                column: "RefDentalProcedureId",
                principalTable: "RefDentalProcedures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
