using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;

namespace YemekSiparis.Application.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public List<string> Products { get; set; }
        }

    }

