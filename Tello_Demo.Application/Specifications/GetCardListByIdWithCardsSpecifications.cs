using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tello_Demo.Domain.Models;

namespace Tello_Demo.Application.Specifications;

public class GetCardListByIdWithCardsSpecifications : Specification<CardList>
{
    public GetCardListByIdWithCardsSpecifications(int id)
    {
        Query.Where(x => x.Id == id)
        .Include(x => x.Cards);
    }
}
