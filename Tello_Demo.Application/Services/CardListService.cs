﻿using AutoMapper;
using FluentResults;
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

    public async Task<Result<CardListDTO>> CreateCardListAsync(CardListDTO cardListDTO)
    {
        try
        {
            CardList card = _mapper.Map<CardListDTO, CardList>(cardListDTO);
            CardList response = await _repo.AddAsync(card);
            CardListDTO resultDto = _mapper.Map<CardList, CardListDTO>(response);

            return Result.Ok(resultDto);
        }
        catch (Exception ex)
        { 
            return Result.Fail(new Error(ex.Message));
        }
    }
}
