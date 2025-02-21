using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeWork.Migrations
{
    /// <inheritdoc />
    public partial class ReferenceIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("4e4dff86-51de-432d-822f-1845ad52772e"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("80874d68-1511-4465-bd9f-ca08aa179933"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("832081d0-5a02-492f-8aab-785d9d4b36fc"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("9b64ec7f-9b58-40e9-b910-6cd44953cea4"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("d30941ae-b428-4c42-ba55-079678747851"));

            migrationBuilder.DeleteData(
                table: "RefStatusType",
                keyColumn: "Id",
                keyValue: new Guid("4023db95-1762-4b3b-a9ba-8484586a42f2"));

            migrationBuilder.DeleteData(
                table: "RefStatusType",
                keyColumn: "Id",
                keyValue: new Guid("64f98627-5932-4c00-b365-d36579af5b9f"));

            migrationBuilder.DeleteData(
                table: "RefStatusType",
                keyColumn: "Id",
                keyValue: new Guid("b87bdb63-8efa-410d-9750-f3a61bf98796"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProcedureRegistrationCards");

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "ProcedureRegistrationCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "422ca248-c208-415b-9f17-c8890af936cc", "AQAAAAIAAYagAAAAEEc/wJ6QW/a4aGOlwSS4urcR3UmYhzV4txtibUG+1yDeRUFcknsIE87uBufLXWoBaQ==" });

            migrationBuilder.InsertData(
                table: "RefDentalProcedures",
                columns: new[] { "Id", "DurationInMinutes", "Name" },
                values: new object[,]
                {
                    { new Guid("09bdd887-063c-4ee3-ba82-2c73eacf0a15"), 60, "C" },
                    { new Guid("3296414b-4689-4707-8121-576055b90b25"), 30, "A" },
                    { new Guid("628a398f-9fc8-49b7-a1d5-9b45b598b8e3"), 45, "B" },
                    { new Guid("640c6798-f815-491b-8d44-1e05977fbc17"), 20, "D" },
                    { new Guid("968c1989-edcb-4e96-9e29-61cd504d66a5"), 40, "E" }
                });

            migrationBuilder.InsertData(
                table: "RefStatusType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1712fff0-4895-42c0-97c3-22c927098391"), "Pending" },
                    { new Guid("7606139b-c894-48f4-9031-b80017e89b84"), "Cancelled" },
                    { new Guid("fdcfd83f-3289-44dc-920e-eb49c734c490"), "Confirmed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRegistrationCards_StatusId",
                table: "ProcedureRegistrationCards",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureRegistrationCards_RefStatusType_StatusId",
                table: "ProcedureRegistrationCards",
                column: "StatusId",
                principalTable: "RefStatusType",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureRegistrationCards_RefStatusType_StatusId",
                table: "ProcedureRegistrationCards");

            migrationBuilder.DropIndex(
                name: "IX_ProcedureRegistrationCards_StatusId",
                table: "ProcedureRegistrationCards");

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("09bdd887-063c-4ee3-ba82-2c73eacf0a15"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("3296414b-4689-4707-8121-576055b90b25"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("628a398f-9fc8-49b7-a1d5-9b45b598b8e3"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("640c6798-f815-491b-8d44-1e05977fbc17"));

            migrationBuilder.DeleteData(
                table: "RefDentalProcedures",
                keyColumn: "Id",
                keyValue: new Guid("968c1989-edcb-4e96-9e29-61cd504d66a5"));

            migrationBuilder.DeleteData(
                table: "RefStatusType",
                keyColumn: "Id",
                keyValue: new Guid("1712fff0-4895-42c0-97c3-22c927098391"));

            migrationBuilder.DeleteData(
                table: "RefStatusType",
                keyColumn: "Id",
                keyValue: new Guid("7606139b-c894-48f4-9031-b80017e89b84"));

            migrationBuilder.DeleteData(
                table: "RefStatusType",
                keyColumn: "Id",
                keyValue: new Guid("fdcfd83f-3289-44dc-920e-eb49c734c490"));

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ProcedureRegistrationCards");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ProcedureRegistrationCards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "311c71b0-151d-4497-985f-7b22605c7053", "AQAAAAIAAYagAAAAEA41FAFlndkpP6PJHF9kjUfB0z5ptzRIfOyrQRlL0zjNm7Dl2/GPD/e8AP27fM2Zow==" });

            migrationBuilder.InsertData(
                table: "RefDentalProcedures",
                columns: new[] { "Id", "DurationInMinutes", "Name" },
                values: new object[,]
                {
                    { new Guid("4e4dff86-51de-432d-822f-1845ad52772e"), 20, "D" },
                    { new Guid("80874d68-1511-4465-bd9f-ca08aa179933"), 40, "E" },
                    { new Guid("832081d0-5a02-492f-8aab-785d9d4b36fc"), 45, "B" },
                    { new Guid("9b64ec7f-9b58-40e9-b910-6cd44953cea4"), 60, "C" },
                    { new Guid("d30941ae-b428-4c42-ba55-079678747851"), 30, "A" }
                });

            migrationBuilder.InsertData(
                table: "RefStatusType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4023db95-1762-4b3b-a9ba-8484586a42f2"), "Cancelled" },
                    { new Guid("64f98627-5932-4c00-b365-d36579af5b9f"), "Pending" },
                    { new Guid("b87bdb63-8efa-410d-9750-f3a61bf98796"), "Confirmed" }
                });
        }
    }
}
