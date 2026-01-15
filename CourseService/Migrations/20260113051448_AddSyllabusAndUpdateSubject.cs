using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseService.Migrations
{
    /// <inheritdoc />
    public partial class AddSyllabusAndUpdateSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Goals",
                table: "Syllabi",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Syllabi",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "AssignmentWeight",
                table: "Syllabi",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExamWeight",
                table: "Syllabi",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Syllabi",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PassGrade",
                table: "Syllabi",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Subjects",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentWeight",
                table: "Syllabi");

            migrationBuilder.DropColumn(
                name: "ExamWeight",
                table: "Syllabi");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Syllabi");

            migrationBuilder.DropColumn(
                name: "PassGrade",
                table: "Syllabi");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Syllabi",
                newName: "Goals");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Syllabi",
                newName: "Content");
        }
    }
}
