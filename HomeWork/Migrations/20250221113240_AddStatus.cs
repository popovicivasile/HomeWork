using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeWork.Migrations
{
    /// <inheritdoc />
    public partial class AddStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "RefStatusType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefStatusType", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefStatusType");

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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "aab7b6cd-5701-49ce-921f-21c63d58bd43", "AQAAAAIAAYagAAAAEMZiqYlPTm4eLaX/aUU83NdMeiet32tsTH5XuDKPdPwkQ2mnvDy1AIJ+59c5tpvCKg==" });

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
        }
    }
}
