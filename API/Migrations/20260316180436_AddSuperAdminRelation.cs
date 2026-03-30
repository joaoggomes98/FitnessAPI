using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddSuperAdminRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NutritionPlans_AspNetUsers_UserId",
                table: "NutritionPlans");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NutritionPlans",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_NutritionPlans_UserId",
                table: "NutritionPlans",
                newName: "IX_NutritionPlans_ClientId");

            migrationBuilder.AddColumn<string>(
                name: "SuperAdminId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SuperAdminId",
                table: "AspNetUsers",
                column: "SuperAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_SuperAdminId",
                table: "AspNetUsers",
                column: "SuperAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NutritionPlans_AspNetUsers_ClientId",
                table: "NutritionPlans",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_SuperAdminId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_NutritionPlans_AspNetUsers_ClientId",
                table: "NutritionPlans");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SuperAdminId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SuperAdminId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "NutritionPlans",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NutritionPlans_ClientId",
                table: "NutritionPlans",
                newName: "IX_NutritionPlans_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NutritionPlans_AspNetUsers_UserId",
                table: "NutritionPlans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
