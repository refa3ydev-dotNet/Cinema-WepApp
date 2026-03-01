using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIsApprovedToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Cinemas");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "Cinemas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Cinemas");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Cinemas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
