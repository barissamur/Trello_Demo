namespace Tello_Demo.Domain.Models;

public class Card : BaseEntity
{
    public int CardListId { get; set; }
    public CardList? CardList { get; set; } 
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int Index { get; set; }
}
