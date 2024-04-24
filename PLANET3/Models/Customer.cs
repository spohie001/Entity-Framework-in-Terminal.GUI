using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace PLANET3.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        //public ICollection<Order> Orders { get; set; } = null!;
        public decimal PriceOfAll()
        {
            return 0.0M;
        }
    }
}
