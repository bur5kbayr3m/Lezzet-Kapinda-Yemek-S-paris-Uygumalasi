using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OrderReportDto
{
    public int StoreId { get; set; }
    public string StoreName { get; set; }

    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }

    public double AverageSpeedRating { get; set; }
    public double AverageTasteRating { get; set; }
    public double AverageServiceRating { get; set; }
}
