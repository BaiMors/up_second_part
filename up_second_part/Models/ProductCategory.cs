using System;
using System.Collections.Generic;

namespace up_second_part.Models;

public partial class ProductCategory
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
