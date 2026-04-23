using Domain.Enumerators;

namespace Domain.Data.Models;

public class CreatePaymentRequest
{
    public decimal TransactionAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public int? Installments { get; set; }
    public string? ExternalReference { get; set; }
    public string? CurrencyId { get; set; }
}
