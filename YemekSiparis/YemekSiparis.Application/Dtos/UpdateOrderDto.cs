using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using YemekSiparis.Domain.Enums;

using System.Collections.Generic;
using YemekSiparis.Domain.Enums;

namespace YemekSiparis.Application.Dtos
{
    public class UpdateOrderDto
    {
        //public OrderStatus Status { get; set; }
        public string Status { get; set; }
        public bool IsPaid { get; set; }
        public List<UpdateOrderItemDto> OrderItems { get; set; }
        public string? StoreNote { get; set; }

    }

    public class UpdateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
