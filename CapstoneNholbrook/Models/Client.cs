namespace CapstoneNHolbrook.Models
{


    using System.ComponentModel.DataAnnotations;

    public class Client
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool CanText { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }


        public string FullName => $"{FirstName} {LastName}";

    }

   

}