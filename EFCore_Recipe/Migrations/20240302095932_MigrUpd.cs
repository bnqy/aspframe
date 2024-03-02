using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore_Recipe.Migrations
{
    /// <inheritdoc />
    public partial class MigrUpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVegan",
                table: "Recipes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVegetarian",
                table: "Recipes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "JustMigrate",
                table: "Ingredient",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVegan",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "IsVegetarian",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "JustMigrate",
                table: "Ingredient");
        }
    }
}
