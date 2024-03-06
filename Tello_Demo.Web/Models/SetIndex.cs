namespace Tello_Demo.Web.Models;

public class SetIndex
{
    public int ListId { get; set; }

    public int ListIndex { get; set; }

    public Dictionary<int, int>? CardIdNewIndex { get; set; } = [];
}
