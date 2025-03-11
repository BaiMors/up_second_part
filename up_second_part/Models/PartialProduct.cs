using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace up_second_part.Models
{
    public partial class Product
    {
        public string DiscountColor
        {
            get
            {
                if (ProductDiscountAmount > 15)
                {
                    return "#7fff00";
                }
                else
                {
                    return "#ffffff";
                }
            }
        }

        public decimal ReducedCost => ProductCost - (ProductCost / 100 * (decimal)ProductDiscountAmount);
    }
}
