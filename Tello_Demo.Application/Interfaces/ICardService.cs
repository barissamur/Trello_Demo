using FluentResults;
using Tello_Demo.Application.DTOs;
using Tello_Demo.Domain.Models;

namespace Tello_Demo.Application.Interfaces;

public interface ICardService
{
    Task<Result<CardDTO>> CreateCardAsync(CardDTO cardDTO);

    Task<Result<IEnumerable<CardDTO>>> CreateRangeCardAsync(List<CardDTO> cardDTOs);

    Task<Result<CardDTO>> GetCardByIdAsync(int id); 
}
