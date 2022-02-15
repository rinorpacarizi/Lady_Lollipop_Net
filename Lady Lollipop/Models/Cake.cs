using Lady_Lollipop.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lady_Lollipop.Models
{
    public class Cake:IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Picture is Required")]
        public string Picture { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public string Ingridients { get; set; }
    }
}
