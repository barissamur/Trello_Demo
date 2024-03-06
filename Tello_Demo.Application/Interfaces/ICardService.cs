using FluentResults;
using Tello_Demo.Application.DTOs;
using Tello_Demo.Domain.Models;

namespace Tello_Demo.Application.Interfaces;

public interface ICardService
{
    Task<Result<CardDTO>> CreateCardAsync(CardDTO cardDTO);
    Task<Result<CardDTO>> UpdateCardAsync(CardDTO cardDTO);
    Task<Result<IEnumerable<CardDTO>>> UpdateRangeCardAsync(List<CardDTO> cardDTOs);
    Task<Result<IEnumerable<CardDTO>>> CreateRangeCardAsync(List<CardDTO> cardDTOs);
    Task<Result<CardDTO>> GetCardByIdAsync(int id);
    Task<Result> DeleteCardAsync(CardDTO cardDTO);
}
