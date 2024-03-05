using FluentResults;
using Tello_Demo.Application.DTOs;
using Tello_Demo.Domain.Models;

namespace Tello_Demo.Application.Interfaces;

public interface ICardListService
{
    Task<Result<CardListDTO>> CreateCardListAsync(CardListDTO cardList);
}
