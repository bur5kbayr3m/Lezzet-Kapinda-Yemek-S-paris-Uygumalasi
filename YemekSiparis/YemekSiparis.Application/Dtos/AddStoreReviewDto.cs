using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AddStoreReviewDto
{
    public int OrderId { get; set; }
    public int StoreId { get; set; }

    public int SpeedRating { get; set; }
    public int TasteRating { get; set; }
    public int ServiceRating { get; set; }

    public string? Comment { get; set; }
}
