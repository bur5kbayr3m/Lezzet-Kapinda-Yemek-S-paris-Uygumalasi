using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UsernameChangeRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }  
    public string UserType { get; set; } 

    public string NewUsername { get; set; }
    public string Status { get; set; } = "Pending"; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
