using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RailwayReservationSystem.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Train",
                columns: table => new
                {
                    TrainId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    TrainNo = table.Column<int>(type: "int", nullable: false),
                    TrainName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceStation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationStation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceDepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DestinationArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalSeat = table.Column<int>(type: "int", nullable: false),
                    AvailableGeneralSeat = table.Column<int>(type: "int", nullable: false),
                    AvailableLadiesSeat = table.Column<int>(type: "int", nullable: false),
                    SeatFare = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Train", x => x.TrainId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Passenger",
                columns: table => new
                {
                    PassengerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passenger", x => x.PassengerId);
                    table.ForeignKey(
                        name: "FK_Passenger_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "100, 1"),
                    TrainId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PassengerId = table.Column<int>(type: "int", nullable: false),
                    PnrNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalFare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionNumber = table.Column<long>(type: "bigint", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuotaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seatNumber = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservation_Passenger_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passenger",
                        principalColumn: "PassengerId");
                    table.ForeignKey(
                        name: "FK_Reservation_Train_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Train",
                        principalColumn: "TrainId");
                    table.ForeignKey(
                        name: "FK_Reservation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passenger_UserId",
                table: "Passenger",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_PassengerId",
                table: "Reservation",
                column: "PassengerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TrainId",
                table: "Reservation",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_UserId",
                table: "Reservation",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Passenger");

            migrationBuilder.DropTable(
                name: "Train");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
