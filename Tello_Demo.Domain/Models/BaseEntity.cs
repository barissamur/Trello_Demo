using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tello_Demo.Domain.Models;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// aktif mi pasif mi
    /// </summary>
    public bool IA { get; set; }
}
