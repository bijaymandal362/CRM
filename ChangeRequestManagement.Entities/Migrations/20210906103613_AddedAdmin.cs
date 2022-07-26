using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChangeRequestManagement.Entities.Migrations
{
    public partial class AddedAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "PersonId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$hGqfprCgzRGPEZTQaSMRa.fan93nqnS1ruKDBmWcp6GjQORPnXZo6");

            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "PersonId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$4j65O6fm6.JuOuxOqLWuk.rUUTdvWrA96GtOnjenTMj7c90I9Fira");

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "PersonId", "Address", "EmailAddress", "FirstName", "InsertDate", "InsertPersonId", "LastName", "Password", "PhoneNumber", "RoleId", "UpdateDate", "UpdatePersonId" },
                values: new object[] { 789, null, "anuj.tamrakar@ekbana.info", "Anuj", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Tamrakar", "$2a$11$f8q.VhadeugNavQHT5VayOuGXifMNMBQy4yw7R4.rpqXb/KLp4TBW", null, 1, null, null });

            migrationBuilder.InsertData(
                table: "Admin",
                columns: new[] { "AdminId", "InsertDate", "InsertPersonId", "PersonId", "UpdateDate", "UpdatePersonId" },
                values: new object[] { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 789, null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "PersonId",
                keyValue: 789);

            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "PersonId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$EPhKEyZhg6zjMo/GwucTmuXJQ2gcV7cpuJqiGr.PfaV5m3oezeONO");

            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "PersonId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$qO/lmfuBeiHpEeuNGVBqquAOMHSrc9vnyUwVfO9v019TEPZP5NDyq");
        }
    }
}
