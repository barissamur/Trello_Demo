using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tello_Demo.Domain.Models;

public class CardList : BaseEntity
{
    public int AccountId { get; set; }
    public string Title { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int Index { get; set; }

    public List<Card> Cards { get; set; } = [];
}
