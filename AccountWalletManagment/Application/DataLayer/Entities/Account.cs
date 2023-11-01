using System.ComponentModel.DataAnnotations;


namespace Application.DataLayer.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string IdCard { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set;}   

    }
}
