using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DBInjectionWithMultiProviders.Plugin1.Migrations.MSSQL
{
    public partial class addedTimeSlotsSql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "loginTime",
                table: "Security",
                type: "datetime",
                nullable: true
                //,defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                );

            migrationBuilder.AddColumn<DateTime>(
                name: "logoutTime",
                table: "Security",
                type: "datetime",
                nullable: true
                //,defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "loginTime",
                table: "Security");

            migrationBuilder.DropColumn(
                name: "logoutTime",
                table: "Security");
        }
        internal void DownManually(MigrationBuilder migrationBuilder)
        {
            Down(migrationBuilder);
        }
    }
}
