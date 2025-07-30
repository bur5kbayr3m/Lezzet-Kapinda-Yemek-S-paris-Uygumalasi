using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YemekSiparis.Domain.Enums
{
    public enum OrderStatus
    {
        Hazirlaniyor = 0,      
        Yolda = 1,             
        TeslimEdildi = 2,      
        OdemeBekleniyor = 3,   
        IptalEdildi = 4       
    }
}
