using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtoKurSkoda.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class t1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "OtoKurSkoda");

            migrationBuilder.CreateTable(
                name: "RoleGroups",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PhoneConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleGroupRoles",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleGroupRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleGroupRoles_RoleGroups_RoleGroupId",
                        column: x => x.RoleGroupId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "RoleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleGroupRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevokedByIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReplacedByToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddresses ",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    AddressLine = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    TaxNumber = table.Column<string>(type: "text", nullable: true),
                    TaxOffice = table.Column<string>(type: "text", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddresses ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddresses _Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_RoleGroups_RoleGroupId",
                        column: x => x.RoleGroupId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "RoleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                schema: "OtoKurSkoda",
                table: "RefreshTokens",
                column: "Token");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "OtoKurSkoda",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupRoles_RoleGroupId",
                schema: "OtoKurSkoda",
                table: "RoleGroupRoles",
                column: "RoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupRoles_RoleId",
                schema: "OtoKurSkoda",
                table: "RoleGroupRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses _UserId",
                schema: "OtoKurSkoda",
                table: "UserAddresses ",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleGroupId",
                schema: "OtoKurSkoda",
                table: "UserRoles",
                column: "RoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                schema: "OtoKurSkoda",
                table: "UserRoles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "RoleGroupRoles",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "UserAddresses ",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "RoleGroups",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "OtoKurSkoda");
        }
    }
}
