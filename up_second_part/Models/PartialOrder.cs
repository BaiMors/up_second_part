using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace up_second_part.Models
{
    public partial class Order
    {
        public decimal OrderSum => OrderProducts.Sum(x => x.ProductArticleNumberNavigation.ReducedCost);
        public float OrderDiscountSum => OrderProducts.Sum(x => x.ProductArticleNumberNavigation.ProductDiscountAmount);

        public string OrderColor
        {
            get
            {
                if (OrderProducts.Any(x => x.ProductArticleNumberNavigation.ProductQuantityInStock == 0))
                {
                    return "#ff8c00";
                }
                else if (OrderProducts.All(x => x.ProductArticleNumberNavigation.ProductQuantityInStock > 3))
                {
                    return "#20b2aa";
                }
                else
                {
                    return "#ffffff";
                }
            }
        }
    }
}
