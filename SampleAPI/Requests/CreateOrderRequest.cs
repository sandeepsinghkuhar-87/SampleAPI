using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Requests
{
    public class CreateOrderRequest
    {
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
