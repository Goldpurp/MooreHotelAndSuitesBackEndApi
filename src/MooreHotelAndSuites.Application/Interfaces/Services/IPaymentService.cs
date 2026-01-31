public interface IPaymentService
{
    Task VerifyPaystackAsync(string bookingCode);
    Task ConfirmTransferAsync(string bookingCode);
}
