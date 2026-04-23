using Domain.Data.Entities;
using Domain.Data.Models;
using Domain.Enumerators;
using Domain.Repository;

namespace Domain.Services;

public class PaymentService(
    IPaymentRepository paymentRepository,
    IOrderRepository orderRepository) : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Payment> CreatePaymentAsync(
        string userId,
        string? orderId,
        CreatePaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var order = orderId != null ? await _orderRepository.GetAsync(orderId, cancellationToken) : null;

        var payment = new Payment
        {
            UserId = userId,
            OrderId = orderId,
            PaymentMethod = request.PaymentMethod,
            Status = PaymentStatus.PENDING,
            TransactionAmount = request.TransactionAmount,
            Installments = request.Installments,
            CurrencyId = request.CurrencyId ?? "BRL",
            ShippingAmount = order?.ShippingAmount,
            ExternalReference = request.ExternalReference,
        };

        await _paymentRepository.InsertAsync(payment);

        return payment;
    }

    public async Task<Payment?> GetPaymentByIdAsync(string paymentId, CancellationToken cancellationToken = default)
    {
        return await _paymentRepository.GetAsync(paymentId, cancellationToken);
    }

    public async Task<Payment?> GetPaymentDetailAsync(string paymentId, CancellationToken cancellationToken = default)
    {
        return await _paymentRepository.GetByIdWithRelationsAsync(paymentId, cancellationToken);
    }

    public async Task<List<Payment>> GetUserPaymentsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _paymentRepository.GetByUserIdAsync(userId, cancellationToken);
    }

    public async Task<List<Payment>> GetAllPaymentsAsync(CancellationToken cancellationToken = default)
    {
        return await _paymentRepository.GetAllWithRelationsAsync(cancellationToken);
    }

    public async Task<Payment> UpdatePaymentStatusAsync(
        string paymentId,
        PaymentStatus newStatus,
        string? statusDetail = null,
        CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetAsync(paymentId, cancellationToken)
            ?? throw new Exception($"Payment {paymentId} not found");

        payment.Status = newStatus;
        payment.StatusDetail = statusDetail;
        payment.DateLastUpdated = DateTime.UtcNow;

        if (newStatus == PaymentStatus.APPROVED)
            payment.DateApproved = DateTime.UtcNow;

        await _paymentRepository.UpdateAsync(payment);

        return payment;
    }
}
