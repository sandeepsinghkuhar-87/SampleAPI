using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; }
        public bool InvoiceStatus { get; set; } = true;
        public bool DeleteStatus { get; set; } = false;
    }
}
