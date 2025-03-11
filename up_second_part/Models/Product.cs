using System;
using System.Collections.Generic;

namespace up_second_part.Models;

public partial class Product
{
    public string ProductArticleNumber { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public int ProductCategory { get; set; }

    public string? ProductPhoto { get; set; }

    public int ProductManufacturer { get; set; }

    public decimal ProductCost { get; set; }

    public float ProductDiscountAmount { get; set; }

    public int ProductQuantityInStock { get; set; }

    public string? ProductStatus { get; set; }

    public float ProductMaxDiscountAmount { get; set; }

    public int ProductMeasurementUnit { get; set; }

    public int ProductSupplier { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ProductCategory ProductCategoryNavigation { get; set; } = null!;

    public virtual Manufacturer ProductManufacturerNavigation { get; set; } = null!;

    public virtual MeasurementUnit ProductMeasurementUnitNavigation { get; set; } = null!;

    public virtual Supplier ProductSupplierNavigation { get; set; } = null!;
}
