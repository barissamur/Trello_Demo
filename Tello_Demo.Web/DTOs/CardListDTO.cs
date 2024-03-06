namespace Tello_Demo.Web.DTOs;

public class CardListDTO 
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Title { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int Index { get; set; }

    public List<CardDTO> Cards { get; set; } = [];
}
