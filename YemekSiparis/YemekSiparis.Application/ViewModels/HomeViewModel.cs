using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;

namespace YemekSiparis.Application.ViewModels
{
    public class HomeViewModel
    {
        public List<StoreDto> PopularStores { get; set; } = new List<StoreDto>();
        public List<StoreDto> AllStores { get; set; } = new();
    }
}