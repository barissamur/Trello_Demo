
using Tello_Demo.Web.DTOs;

namespace Tello_Demo.Web.Models;

public class IndexVM
{
    public int AccountId { get; set; }

    public IEnumerable<CardListDTO> cardLists { get; set; } = [];
}
