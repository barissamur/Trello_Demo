using AutoMapper;
using FluentResults;
using Tello_Demo.Application.DTOs;
using Tello_Demo.Application.Interfaces;
using Tello_Demo.Domain.Models;

namespace Tello_Demo.Application.Services;

public class CardService : ICardService
{
    private readonly IRepo<Card> _repo;
    private readonly IMapper _mapper;

    public CardService(IRepo<Card> repo
        , IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<CardDTO>> CreateCardAsync(CardDTO cardDTO)
    {
        try
        {
            Card card = _mapper.Map<CardDTO, Card>(cardDTO);
            Card response = await _repo.AddAsync(card);
            CardDTO resultDto = _mapper.Map<Card, CardDTO>(response);
         
            return Result.Ok(resultDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<CardDTO>>> CreateRangeCardAsync(List<CardDTO> cardDTOs)
    {
        try
        {
            IEnumerable<Card> cards = cardDTOs.Select(x => _mapper.Map<CardDTO, Card>(x));
            IEnumerable<CardDTO> resultDto = cards.Select(x => _mapper.Map<Card, CardDTO>(x));

            return Result.Ok(resultDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error(ex.Message));
        }

    }
}
