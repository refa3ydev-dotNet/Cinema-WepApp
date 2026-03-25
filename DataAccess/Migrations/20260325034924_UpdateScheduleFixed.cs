using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieSchedules_Rooms_RoomId",
                table: "MovieSchedules");

            migrationBuilder.RenameColumn(
                name: "seatCount",
                table: "Rooms",
                newName: "SeatCount");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "MovieSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "MovieSchedules",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieSchedules_Rooms_RoomId",
                table: "MovieSchedules",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieSchedules_Rooms_RoomId",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "MovieSchedules");

            migrationBuilder.RenameColumn(
                name: "SeatCount",
                table: "Rooms",
                newName: "seatCount");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "MovieSchedules",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieSchedules_Rooms_RoomId",
                table: "MovieSchedules",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
