namespace Tello_Demo.Web.Models;

public class Card 
{
    public int Id { get; set; } 
    public CardList? CardList { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Type { get; set; } 
    public int Index { get; set; }
}
