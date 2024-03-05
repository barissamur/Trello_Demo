using AutoMapper;
using Tello_Demo.Application.DTOs;
using Tello_Demo.Application.Interfaces;
using Tello_Demo.Domain.Models;

namespace Tello_Demo.Application.Services;

public class CardListService : ICardListService
{
    private readonly IRepo<CardList> _repo;
    private readonly IMapper _mapper;

    public CardListService(IRepo<CardList> repo
        , IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task CreateCardListAsync(CardListDTO cardListDTO)
    {
        CardList card = _mapper.Map<CardListDTO, CardList>(cardListDTO);

        await _repo.AddAsync(card);
    }
}
