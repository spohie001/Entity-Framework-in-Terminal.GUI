using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLANET3.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public int CustomerId { get; set; }
        public ICollection<Item> Items { get; } = new List<Item>();


        public decimal TotalPrice()
        {
            decimal totalPrice = 0;
            foreach (var i in Items)
            {
                totalPrice += i.Price;
            }
            return totalPrice;
        }

        public int AmountOfItems()
        {
            int amountOfItems = 0;
            foreach (var i in Items)
            {
                amountOfItems += 1;
            }
            return amountOfItems;
        }
    }
}
