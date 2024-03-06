using AutoMapper;
using FluentResults;
using Tello_Demo.Application.DTOs;
using Tello_Demo.Application.Interfaces;
using Tello_Demo.Application.Specifications;
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
            card.CardList = null;
            Card response = await _repo.AddAsync(card);
            CardDTO resultDto = _mapper.Map<Card, CardDTO>(response);

            return Result.Ok(resultDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error(ex.Message));
        }
    }

    public Task<Result<IEnumerable<CardDTO>>> CreateRangeCardAsync(List<CardDTO> cardDTOs)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteCardAsync(CardDTO cardDTO)
    {
        try
        {
            Card card = await _repo.GetByIdAsync(cardDTO.Id);
            await _repo.DeleteAsync(card);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error(ex.Message));
        }
    }

    public async Task<Result<CardDTO>> GetCardByIdAsync(int id)
    {
        try
        {
            GetCardByIdWithCardListSpecification getCardByIdWithCardListSpecification = new(id);

            Card card = await _repo.GetBySpecAsync(getCardByIdWithCardListSpecification);

            CardDTO resultDto = _mapper.Map<Card, CardDTO>(card);

            return Result.Ok(resultDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error(ex.Message));
        }
    }

    public Task<Result<CardDTO>> UpdateCardAsync(CardDTO cardDTO)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<CardDTO>>> UpdateRangeCardAsync(List<CardDTO> cardDTOs)
    {
        throw new NotImplementedException();

    }
}
