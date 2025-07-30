using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekSiparis.Domain.Entities;

public class StoreReview
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public int StoreId { get; set; }
    public int OrderId { get; set; }

    public int SpeedRating { get; set; }     
    public int TasteRating { get; set; }     
    public int ServiceRating { get; set; }   

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
    public Store Store { get; set; }
    public Order Order { get; set; }
}
