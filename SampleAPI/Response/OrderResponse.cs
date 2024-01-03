using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Response
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
