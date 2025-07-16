using System.ComponentModel.DataAnnotations;

namespace NetCoreAI.Project1_ApiDemo.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerSurname { get; set; }
        public decimal CustomerBalance { get; set; }

    }
}
