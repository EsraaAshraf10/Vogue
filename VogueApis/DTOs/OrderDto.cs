using System.ComponentModel.DataAnnotations;
using VogueCore.Entities.Identity;

namespace VogueApis.DTOs
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto shipToAddress { get; set; }
    }
}
