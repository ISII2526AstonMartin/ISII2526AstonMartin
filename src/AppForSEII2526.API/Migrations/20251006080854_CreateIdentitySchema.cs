using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppForSEII2526.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateIdentitySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Compra",
                columns: table => new
                {
                    CompraID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    nBocadillo = table.Column<int>(type: "int", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecioTotal = table.Column<float>(type: "real", nullable: false),
                    Apellido_1Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido_2Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetodoPago = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compra", x => x.CompraID);
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    CompraID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Apellido1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion_Envio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Metodo_Pago = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecioFinal = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.CompraID);
                });

            migrationBuilder.CreateTable(
                name: "ComprasBono",
                columns: table => new
                {
                    CompraBonoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido1Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido2Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCompraBono = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NBono = table.Column<int>(type: "int", nullable: false),
                    PrecioTotalBono = table.Column<float>(type: "real", nullable: false),
                    MetodoPagoUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasBono", x => x.CompraBonoId);
                });

            migrationBuilder.CreateTable(
                name: "Resenyas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValoracionGeneral = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resenyas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoPan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoPan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposBocadillos",
                columns: table => new
                {
                    IdTipo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NombreTipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposBocadillos", x => x.IdTipo);
                });

            migrationBuilder.CreateTable(
                name: "TiposProductos",
                columns: table => new
                {
                    ProductoID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposProductos", x => x.ProductoID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bocadillo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PVP = table.Column<float>(type: "real", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    TipoPanId = table.Column<int>(type: "int", nullable: false),
                    Tamanyo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bocadillo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bocadillo_TipoPan_TipoPanId",
                        column: x => x.TipoPanId,
                        principalTable: "TipoPan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BonoBocadillos",
                columns: table => new
                {
                    BonoID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CantidadDisponible = table.Column<int>(type: "int", nullable: false),
                    NBocadillos = table.Column<int>(type: "int", nullable: false),
                    NombreBocadillo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PVP = table.Column<float>(type: "real", nullable: false),
                    TipoIdTipo = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonoBocadillos", x => x.BonoID);
                    table.ForeignKey(
                        name: "FK_BonoBocadillos_TiposBocadillos_TipoIdTipo",
                        column: x => x.TipoIdTipo,
                        principalTable: "TiposBocadillos",
                        principalColumn: "IdTipo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    ProductoID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PVP = table.Column<float>(type: "real", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Tipo_ProductoProductoID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.ProductoID);
                    table.ForeignKey(
                        name: "FK_Productos_TiposProductos_Tipo_ProductoProductoID",
                        column: x => x.Tipo_ProductoProductoID,
                        principalTable: "TiposProductos",
                        principalColumn: "ProductoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompraBocadillo",
                columns: table => new
                {
                    BocadilloId = table.Column<int>(type: "int", nullable: false),
                    CompraId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<float>(type: "real", nullable: false),
                    TipoPan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreBocadillo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraBocadillo", x => new { x.BocadilloId, x.CompraId });
                    table.ForeignKey(
                        name: "FK_CompraBocadillo_Bocadillo_BocadilloId",
                        column: x => x.BocadilloId,
                        principalTable: "Bocadillo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompraBocadillo_Compra_CompraId",
                        column: x => x.CompraId,
                        principalTable: "Compra",
                        principalColumn: "CompraID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResenyaBocadillos",
                columns: table => new
                {
                    BocadilloId = table.Column<int>(type: "int", nullable: false),
                    ResenyaId = table.Column<int>(type: "int", nullable: false),
                    Puntuacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResenyaBocadillos", x => new { x.BocadilloId, x.ResenyaId });
                    table.ForeignKey(
                        name: "FK_ResenyaBocadillos_Bocadillo_BocadilloId",
                        column: x => x.BocadilloId,
                        principalTable: "Bocadillo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResenyaBocadillos_Resenyas_ResenyaId",
                        column: x => x.ResenyaId,
                        principalTable: "Resenyas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BonosComprados",
                columns: table => new
                {
                    BonoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompraId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompraBonoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioBono = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonosComprados", x => new { x.BonoId, x.CompraId });
                    table.ForeignKey(
                        name: "FK_BonosComprados_BonoBocadillos_BonoId",
                        column: x => x.BonoId,
                        principalTable: "BonoBocadillos",
                        principalColumn: "BonoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonosComprados_ComprasBono_CompraBonoId",
                        column: x => x.CompraBonoId,
                        principalTable: "ComprasBono",
                        principalColumn: "CompraBonoId");
                });

            migrationBuilder.CreateTable(
                name: "Productos_Compras",
                columns: table => new
                {
                    CompraID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductoID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PVP = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos_Compras", x => new { x.CompraID, x.ProductoID });
                    table.ForeignKey(
                        name: "FK_Productos_Compras_Compras_CompraID",
                        column: x => x.CompraID,
                        principalTable: "Compras",
                        principalColumn: "CompraID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Productos_Compras_Productos_ProductoID",
                        column: x => x.ProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bocadillo_TipoPanId",
                table: "Bocadillo",
                column: "TipoPanId");

            migrationBuilder.CreateIndex(
                name: "IX_BonoBocadillos_TipoIdTipo",
                table: "BonoBocadillos",
                column: "TipoIdTipo");

            migrationBuilder.CreateIndex(
                name: "IX_BonosComprados_CompraBonoId",
                table: "BonosComprados",
                column: "CompraBonoId");

            migrationBuilder.CreateIndex(
                name: "IX_CompraBocadillo_CompraId",
                table: "CompraBocadillo",
                column: "CompraId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Tipo_ProductoProductoID",
                table: "Productos",
                column: "Tipo_ProductoProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Compras_ProductoID",
                table: "Productos_Compras",
                column: "ProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_ResenyaBocadillos_ResenyaId",
                table: "ResenyaBocadillos",
                column: "ResenyaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BonosComprados");

            migrationBuilder.DropTable(
                name: "CompraBocadillo");

            migrationBuilder.DropTable(
                name: "Productos_Compras");

            migrationBuilder.DropTable(
                name: "ResenyaBocadillos");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BonoBocadillos");

            migrationBuilder.DropTable(
                name: "ComprasBono");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Bocadillo");

            migrationBuilder.DropTable(
                name: "Resenyas");

            migrationBuilder.DropTable(
                name: "TiposBocadillos");

            migrationBuilder.DropTable(
                name: "TiposProductos");

            migrationBuilder.DropTable(
                name: "TipoPan");
        }
    }
}
