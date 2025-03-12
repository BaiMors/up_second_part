using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace up_second_part.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int OrderStatus { get; set; }

    public DateTime OrderDeliveryDate { get; set; }

    public int OrderPickupPoint { get; set; }

    public DateTime OrderDate { get; set; }

    public int OrderClient { get; set; }

    public short OrderReceiptCode { get; set; }

    public virtual User OrderClientNavigation { get; set; } = null!;

    public virtual PickupPoint OrderPickupPointNavigation { get; set; } = null!;

    public virtual ObservableCollection<OrderProduct> OrderProducts { get; set; } = new();

    public virtual OrderStatus OrderStatusNavigation { get; set; } = null!;
}
