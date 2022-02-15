using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lady_Lollipop.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public Sweet Sweet { get; set; }
        public int Amount { get; set; }
        public string ShoppingCartId { get; set; }

    }
}
