using AutoMapper;
using Tello_Demo.Application.DTOs;
using Tello_Demo.Domain.Models;

namespace Tello_Demo.Application.Mapping.CardListMapping;

public class CardListMappingProfile : Profile
{
    public CardListMappingProfile()
    {
        CreateMap<CardList, CardListDTO>()
                .ReverseMap();
    }
}
