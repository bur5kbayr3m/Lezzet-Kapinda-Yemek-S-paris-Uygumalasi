using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YemekSiparis.Application.Dtos;

public interface ICourierService
{
    Task<List<CourierDto>> GetAllCouriersAsync();
    Task AssignCourierAsync(AssignCourierDto dto);
    Task<List<OrderDto>> GetOrdersForCourierAsync(int courierId);
}
