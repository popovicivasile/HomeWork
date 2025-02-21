using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeWork.Migrations
{
    /// <inheritdoc />
    public partial class booking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationInMinutes",
                table: "RefDentalProcedures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProcedureRegistrationCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProcedureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureRegistrationCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedureRegistrationCards_AspNetUsers_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcedureRegistrationCards_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcedureRegistrationCards_RefDentalProcedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "RefDentalProcedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Doctor", "DOCTOR" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "aab7b6cd-5701-49ce-921f-21c63d58bd43", "superuser@gmail.com", "Super", "User", "SUPERUSER@GMAIL.COM", "SUPERUSER", "AQAAAAIAAYagAAAAEMZiqYlPTm4eLaX/aUU83NdMeiet32tsTH5XuDKPdPwkQ2mnvDy1AIJ+59c5tpvCKg==", "SuperUser" });

            migrationBuilder.InsertData(
                table: "RefDentalProcedures",
                columns: new[] { "Id", "DurationInMinutes", "Name" },
                values: new object[,]
                {
                    { new Guid("47fb8ba9-a62b-4fa1-b6f8-259c22ce3285"), 20, "D" },
                    { new Guid("7e2c1e72-ba23-4ec9-bde2-908fcae2c92c"), 30, "A" },
                    { new Guid("a480a76b-d78e-4808-bd22-1bc5b3b4f31d"), 45, "B" },
                    { new Guid("c2aff016-0a65-4849-9fc4-bbf5e0435868"), 40, "E" },
                    { new Guid("cafb8d58-b8ba-4459-a705-b78a4593be8b"), 60, "C" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRegistrationCards_DoctorId",
                table: "ProcedureRegistrationCards",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRegistrationCards_PatientId",
                table: "ProcedureRegistrationCards",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRegistrationCards_ProcedureId",
                table: "ProcedureRegistrationCards",
                column: "ProcedureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcedureRegistrationCards");

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("47fb8ba9-a62b-4fa1-b6f8-259c22ce3285"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("7e2c1e72-ba23-4ec9-bde2-908fcae2c92c"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("a480a76b-d78e-4808-bd22-1bc5b3b4f31d"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("c2aff016-0a65-4849-9fc4-bbf5e0435868"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("cafb8d58-b8ba-4459-a705-b78a4593be8b"));

            migrationBuilder.DropColumn(
                name: "DurationInMinutes",
                table: "RefDentalProcedures");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "MedicalProfessional", "MEDICALPROFESSIONAL" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "937de724-6de5-41c8-9187-a155079c94f8", "TemporaryEmail@example.com", "Temporary first Name", "Temporary last Name", "TEMPORARYEMAIL@EXAMPLE.COM", "TEMPORARY-USERNAME", "AQAAAAIAAYagAAAAEGPrngkGQK7hgJJFn2Cdw6M5RJS2UFpTPqzhI4QGzE+phbhvnzZvBipPfI7huUUbaw==", "TemporaryUsername" });
        }
    }
}
