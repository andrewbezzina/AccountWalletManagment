
using System.ComponentModel.DataAnnotations;


namespace Application.DataLayer.Entities
{
    public class Wallet
    {
        [Key]
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
