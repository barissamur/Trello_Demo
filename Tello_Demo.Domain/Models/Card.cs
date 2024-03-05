using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tello_Demo.Domain.Models;

public class Card : BaseEntity
{
    public CardList List { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int Index { get; set; } 
}
