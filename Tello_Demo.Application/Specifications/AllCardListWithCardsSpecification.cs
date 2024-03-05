using Ardalis.Specification;
using Tello_Demo.Domain.Models;

namespace Tello_Demo.Application.Specifications;

public class AllCardListWithCardsSpecification : Specification<CardList>
{
    public AllCardListWithCardsSpecification()
    {
        Query.Include(x => x.Cards);
    }
}
