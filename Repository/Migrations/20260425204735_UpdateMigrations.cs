using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBCartItem");

            migrationBuilder.DropTable(
                name: "TBCoupon");

            migrationBuilder.DropTable(
                name: "TBGiftCard");

            migrationBuilder.DropTable(
                name: "TBOrderItem");

            migrationBuilder.DropTable(
                name: "TBUserAddress");

            migrationBuilder.DropTable(
                name: "TBWishlistItem");

            migrationBuilder.DropTable(
                name: "TBCart");

            migrationBuilder.DropTable(
                name: "TBPayment");

            migrationBuilder.DropTable(
                name: "TBProduct");

            migrationBuilder.DropTable(
                name: "TBOrder");

            migrationBuilder.DropColumn(
                name: "Cellphone",
                table: "TBUserHistoric");

            migrationBuilder.DropColumn(
                name: "ReceiveEmailOffers",
                table: "TBUserHistoric");

            migrationBuilder.DropColumn(
                name: "ReceiveWhatsappOffers",
                table: "TBUserHistoric");

            migrationBuilder.DropColumn(
                name: "AvatarPath",
                table: "TBUser");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "TBUser");

            migrationBuilder.DropColumn(
                name: "ReceiveEmailOffers",
                table: "TBUser");

            migrationBuilder.DropColumn(
                name: "ReceiveWhatsappOffers",
                table: "TBUser");

            migrationBuilder.AddColumn<DateOnly>(
                name: "BirthDate",
                table: "TBUserHistoric",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phones",
                table: "TBUserHistoric",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProfileType",
                table: "TBUser",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "TBUserHistoric");

            migrationBuilder.DropColumn(
                name: "Phones",
                table: "TBUserHistoric");

            migrationBuilder.AddColumn<string>(
                name: "Cellphone",
                table: "TBUserHistoric",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ReceiveEmailOffers",
                table: "TBUserHistoric",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiveWhatsappOffers",
                table: "TBUserHistoric",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProfileType",
                table: "TBUser",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AvatarPath",
                table: "TBUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "TBUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiveEmailOffers",
                table: "TBUser",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiveWhatsappOffers",
                table: "TBUser",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TBCart",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBCart_TBUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TBUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TBCoupon",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsageCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCoupon", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBOrder",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdminNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyerCellphone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyerDocument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CouponCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PaymentApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ShippingCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingComplement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingDeliveryTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingNeighborhood = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingService = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingZipcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubTotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SuperFreteLabelUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuperFreteOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TrackingCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBOrder_TBUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TBUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBProduct",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdditionalImagesUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    ImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Length = table.Column<float>(type: "real", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockAmount = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBUserAddress",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Complement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactCellphone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverLastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBUserAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBUserAddress_TBUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TBUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TBPayment",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuthorizationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoletoBarcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoletoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateApproved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateLastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExternalReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Installments = table.Column<int>(type: "int", nullable: true),
                    LiveMode = table.Column<bool>(type: "bit", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    PixCopyPaste = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PixQrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PixQrCodeBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StatementDescriptor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBPayment_TBOrder_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TBOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TBPayment_TBUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TBUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBCartItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CartId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCartItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBCartItem_TBCart_CartId",
                        column: x => x.CartId,
                        principalTable: "TBCart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBCartItem_TBProduct_Produ~",
                        column: x => x.ProductId,
                        principalTable: "TBProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TBOrderItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProductImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBOrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBOrderItem_TBOrder_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TBOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBOrderItem_TBProduct_Prod~",
                        column: x => x.ProductId,
                        principalTable: "TBProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBWishlistItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBWishlistItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBWishlistItem_TBProduct_P~",
                        column: x => x.ProductId,
                        principalTable: "TBProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBWishlistItem_TBUser_User~",
                        column: x => x.UserId,
                        principalTable: "TBUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TBGiftCard",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsedOnOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBGiftCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBGiftCard_TBPayment_Payme~",
                        column: x => x.PaymentId,
                        principalTable: "TBPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TBGiftCard_TBUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TBUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBCart_UserId",
                table: "TBCart",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TBCartItem_CartId",
                table: "TBCartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_TBCartItem_ProductId",
                table: "TBCartItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TBGiftCard_PaymentId",
                table: "TBGiftCard",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_TBGiftCard_UserId",
                table: "TBGiftCard",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TBOrder_UserId",
                table: "TBOrder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TBOrderItem_OrderId",
                table: "TBOrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TBOrderItem_ProductId",
                table: "TBOrderItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TBPayment_OrderId",
                table: "TBPayment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TBPayment_UserId",
                table: "TBPayment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TBUserAddress_UserId",
                table: "TBUserAddress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TBWishlistItem_ProductId",
                table: "TBWishlistItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TBWishlistItem_UserId_Prod~",
                table: "TBWishlistItem",
                columns: new[] { "UserId", "ProductId" });
        }
    }
}
