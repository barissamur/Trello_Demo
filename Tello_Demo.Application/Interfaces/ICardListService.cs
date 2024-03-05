using Tello_Demo.Application.DTOs;

namespace Tello_Demo.Application.Interfaces;

public interface ICardListService 
{
    Task CreateCardListAsync(CardListDTO cardListDTO);
 }
