using System;
using System.Collections.Generic;

namespace up_second_part.Models;

public partial class PickupPoint
{
    public int Id { get; set; }

    public string PickupPointAddress { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
