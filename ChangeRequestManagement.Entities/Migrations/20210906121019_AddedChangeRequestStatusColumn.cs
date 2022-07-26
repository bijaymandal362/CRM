using Microsoft.EntityFrameworkCore.Migrations;

namespace ChangeRequestManagement.Entities.Migrations
{
    public partial class AddedChangeRequestStatusColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChangeRequestStatusId",
                table: "ChangeRequestDocument",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "PersonId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$6YA6PHEKC9.uViUua0geYe2HTisnScKW3hlaPeogog/P.dVlH.Dpu");

            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "PersonId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$ajSDeiiBIg/IuUvNWe0sx.UwQwc6.CRSN5nP60mnEPOMdcWBhlZJO");

            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "PersonId",
                keyValue: 789,
                column: "Password",
                value: "$2a$11$euv2qI0XWKD0ndaPoEPKdOBVqTFeeuGa2762jGiWzt/kx3AI9JY1i");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestDocument_ChangeRequestStatusId",
                table: "ChangeRequestDocument",
                column: "ChangeRequestStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeRequestDocument_ChangeRequestStatus_ChangeRequestStat~",
                table: "ChangeRequestDocument",
                column: "ChangeRequestStatusId",
                principalTable: "ChangeRequestStatus",
                principalColumn: "ChangeRequestStatusId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeRequestDocument_ChangeRequestStatus_ChangeRequestStat~",
                table: "ChangeRequestDocument");

            migrationBuilder.DropIndex(
                name: "IX_ChangeRequestDocument_ChangeRequestStatusId",
                table: "ChangeRequestDocument");

            migrationBuilder.DropColumn(
                name: "ChangeRequestStatusId",
                table: "ChangeRequestDocument");

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

            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "PersonId",
                keyValue: 789,
                column: "Password",
                value: "$2a$11$f8q.VhadeugNavQHT5VayOuGXifMNMBQy4yw7R4.rpqXb/KLp4TBW");
        }
    }
}
