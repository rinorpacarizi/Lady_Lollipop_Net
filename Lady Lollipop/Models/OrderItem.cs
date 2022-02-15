using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lady_Lollipop.Models
{
    public class OrderItem
    {
        [Key]
        public int ItemId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public int Id { get; set; }
        [ForeignKey("Id")]
        public Sweet Sweet { get; set; }
        public int OrderId { get; set; }
        
        public virtual Order Order { get; set; }
    }
}
