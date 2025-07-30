using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekSiparis.Domain.Entities;

public class FavoriteStore
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int StoreId { get; set; }
    public Store Store { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
