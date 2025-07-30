using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;

namespace YemekSiparis.Application.ViewModels
{
    public class StoreMenuViewModel
    {
        public StoreDto Store { get; set; }
        public List<ProductDto> Products { get; set; }
     
    }

}
