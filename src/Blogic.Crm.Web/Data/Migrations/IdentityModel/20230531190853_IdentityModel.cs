using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogic.Crm.Web.Data.Migrations.IdentityModel
{
    /// <inheritdoc />
    public partial class IdentityModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "clients",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    given_name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    family_name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    normalized_email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    is_email_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    is_phone_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    age = table.Column<short>(type: "smallint", nullable: false),
                    birth_number = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    concurrency_stamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    security_stamp = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "consultants",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    given_name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    family_name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    normalized_email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    is_email_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    is_phone_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    age = table.Column<short>(type: "smallint", nullable: false),
                    birth_number = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    concurrency_stamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    security_stamp = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consultants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contract_consultants",
                schema: "dbo",
                columns: table => new
                {
                    ContractId = table.Column<long>(type: "bigint", nullable: false),
                    ConsultantId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contract_consultants", x => new { x.ContractId, x.ConsultantId });
                });

            migrationBuilder.CreateTable(
                name: "contracts",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    registration_number = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    institution = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    date_concluded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_expired = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_valid = table.Column<DateTime>(type: "datetime2", nullable: false),
                    client_id = table.Column<long>(type: "bigint", nullable: false),
                    manager_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contracts", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clients_birth_number",
                schema: "dbo",
                table: "clients",
                column: "birth_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_normalized_email",
                schema: "dbo",
                table: "clients",
                column: "normalized_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_phone",
                schema: "dbo",
                table: "clients",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consultants_birth_number",
                schema: "dbo",
                table: "consultants",
                column: "birth_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consultants_normalized_email",
                schema: "dbo",
                table: "consultants",
                column: "normalized_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consultants_phone",
                schema: "dbo",
                table: "consultants",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contracts_registration_number",
                schema: "dbo",
                table: "contracts",
                column: "registration_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clients",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "consultants",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "contract_consultants",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "contracts",
                schema: "dbo");
        }
    }
}
