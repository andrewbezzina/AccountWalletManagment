
namespace Application.DataLayer.Responses
{
    public record GetTransactionResponse (Guid Uuid, decimal Amount, decimal RemainingBalance, string TransactionReference, DateTime date);
}
