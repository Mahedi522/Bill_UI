using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bill_UI.Models
{
    public partial class Product
    {
        public Product()
        {
            InventoryProducts = new HashSet<InventoryProduct>();
        }
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double Rate { get; set; }

        public virtual ICollection<InventoryProduct>? InventoryProducts { get; set; }
    }
}
