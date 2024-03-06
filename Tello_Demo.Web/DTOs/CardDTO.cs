namespace Tello_Demo.Web.DTOs;

public class CardDTO 
{
    public int Id { get; set; } 
    public CardListDTO? CardList { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int Index { get; set; }
}
