using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;

namespace YemekSiparis.Application.ViewModels;
public class StoreDetailViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string BannerImageUrl { get; set; }
    public string Address { get; set; }
    public string WorkingHours { get; set; }
    public string ContactPhone { get; set; }
    public string ContactMail { get; set; }
    public List<ProductDto> Products { get; set; } = new List<ProductDto>();

}
