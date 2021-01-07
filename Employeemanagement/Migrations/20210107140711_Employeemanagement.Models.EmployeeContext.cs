using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Employeemanagement.Migrations
{
    public partial class EmployeemanagementModelsEmployeeContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CityMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Datecreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_CityMaster_CityId",
                        column: x => x.CityId,
                        principalTable: "CityMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CityMaster",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Bangalore" },
                    { 2, "Mysore" },
                    { 3, "Hyderabad" },
                    { 4, "Vijayawada" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CityId",
                table: "Employee",
                column: "CityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "CityMaster");
        }
    }
}
