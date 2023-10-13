using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TareasMVC.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Cuando creemos la base de datos, siempre tendremos el rol de admin.
            migrationBuilder.Sql(@"
                    IF NOT EXISTS(Select Id from AspNetRoles where Id = '8a89a221-cdf1-470e-ab5f-d0d1f0e809e4')
                    BEGIN
	                    INSERT AspNetRoles (Id, [Name], [NormalizedName])
	                    VALUES ('8a89a221-cdf1-470e-ab5f-d0d1f0e809e4', 'admin', 'ADMIN')
                    END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles Where Id ='8a89a221-cdf1-470e-ab5f-d0d1f0e809e4'");
        }
    }
}
