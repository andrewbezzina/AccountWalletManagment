using System.ComponentModel.DataAnnotations;


namespace Application.DataLayer.Entities
{
    public class WalletTransaction
    {
        [Key]
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public decimal RemainingBalance { get; set; }
        public string TransactionReference { get; set; }
        public DateTime Created { get; set; }
    }
}
