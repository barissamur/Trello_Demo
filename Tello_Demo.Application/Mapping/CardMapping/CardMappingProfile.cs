using AutoMapper;
using Tello_Demo.Application.DTOs;
using Tello_Demo.Domain.Models;

namespace Tello_Demo.Application.Mapping.CardMapping;

public class CardMappingProfile : Profile
{
    public CardMappingProfile()
    {
        CreateMap<Card, CardDTO>()
            .ReverseMap();
     }
}
