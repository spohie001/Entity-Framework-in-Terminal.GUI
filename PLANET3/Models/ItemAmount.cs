using System.ComponentModel.DataAnnotations;

namespace PLANET3.Models
{
    public class ItemAmount
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        public int Amount { get; set; }
    }
}