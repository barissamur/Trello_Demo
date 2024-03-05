using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tello_Demo.Application.Interfaces;
using Tello_Demo.Domain.Models;

namespace Tello_Demo.Application.Services
{
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
    }
}
