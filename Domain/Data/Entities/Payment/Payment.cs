using Domain.Data.Entities._Base;
using Domain.Data.Entities._Base.Extension;
using Domain.Enumerators;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBPayment")]
public class Payment : BaseEntity, IBaseEntity<Payment>
{
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    public string? OrderId { get; set; }
    public Order? Order { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TransactionAmount { get; set; }
    public int? Installments { get; set; }

    public string? CurrencyId { get; set; }
    public string? AuthorizationCode { get; set; }
    public bool? LiveMode { get; set; }
    public string? StatementDescriptor { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ShippingAmount { get; set; }

    public string? ExternalReference { get; set; }
    public string? StatusDetail { get; set; }
    public DateTime? DateApproved { get; set; }
    public DateTime? DateLastUpdated { get; set; }
    public DateTime? DateOfExpiration { get; set; }

    public string? PixQrCode { get; set; }
    public string? PixQrCodeBase64 { get; set; }
    public string? PixCopyPaste { get; set; }

    public string? BoletoUrl { get; set; }
    public string? BoletoBarcode { get; set; }



    #region .: METHODS :.

    public Payment WithoutRelations(Payment entity)
    {
        if (entity == null)
            return null!;

        var newEntity = new Payment()
        {
            UserId = entity.UserId,
            OrderId = entity.OrderId,
            PaymentMethod = entity.PaymentMethod,
            Status = entity.Status,
            TransactionAmount = entity.TransactionAmount,
            Installments = entity.Installments,
            CurrencyId = entity.CurrencyId,
            AuthorizationCode = entity.AuthorizationCode,
            LiveMode = entity.LiveMode,
            StatementDescriptor = entity.StatementDescriptor,
            ShippingAmount = entity.ShippingAmount,
            ExternalReference = entity.ExternalReference,
            StatusDetail = entity.StatusDetail,
            DateApproved = entity.DateApproved,
            DateLastUpdated = entity.DateLastUpdated,
            DateOfExpiration = entity.DateOfExpiration,
            PixQrCode = entity.PixQrCode,
            PixQrCodeBase64 = entity.PixQrCodeBase64,
            PixCopyPaste = entity.PixCopyPaste,
            BoletoUrl = entity.BoletoUrl,
            BoletoBarcode = entity.BoletoBarcode,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion .: METHODS :.
}
