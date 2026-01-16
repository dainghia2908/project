using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClassLecturerColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LecturerId",
                table: "Classes",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LecturerEmail",
                table: "Classes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LecturerName",
                table: "Classes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LecturerEmail",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "LecturerName",
                table: "Classes");

            migrationBuilder.AlterColumn<Guid>(
                name: "LecturerId",
                table: "Classes",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
