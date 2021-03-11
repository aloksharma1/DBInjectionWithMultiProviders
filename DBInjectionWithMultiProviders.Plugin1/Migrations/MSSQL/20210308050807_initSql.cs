using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DBInjectionWithMultiProviders.Plugin1.Migrations.MSSQL
{
    public partial class initSql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "UserInfo",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        DateCreated = table.Column<DateTime>(nullable: false),
            //        DateModified = table.Column<DateTime>(nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        FullName = table.Column<string>(type: "nvarchar(50)", nullable: true),
            //        UserName = table.Column<string>(nullable: true),
            //        Password = table.Column<string>(type: "nvarchar(20)", nullable: true),
            //        Email = table.Column<string>(type: "nvarchar(550)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserInfo", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "Security",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LoginIp = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    LoginDate = table.Column<DateTime>(nullable: false),
                    LoginUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Security", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Security_UserInfo_LoginUserId",
                        column: x => x.LoginUserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Security_LoginUserId",
                table: "Security",
                column: "LoginUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Security");

            //migrationBuilder.DropTable(
            //    name: "UserInfo");
        }
    }
}
