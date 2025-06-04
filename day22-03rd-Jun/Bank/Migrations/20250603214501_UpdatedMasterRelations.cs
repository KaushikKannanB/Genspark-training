using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMasterRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Master",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Master",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Admins_Email",
                table: "Admins");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Name",
                table: "Admins",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Master",
                table: "Admins",
                column: "Name",
                principalTable: "Master",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Master",
                table: "Users",
                column: "Name",
                principalTable: "Master",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Master",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Master",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Name",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Admins_Name",
                table: "Admins");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Email",
                table: "Admins",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Master",
                table: "Admins",
                column: "Email",
                principalTable: "Master",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Master",
                table: "Users",
                column: "Email",
                principalTable: "Master",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
