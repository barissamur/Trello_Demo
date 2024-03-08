using Microsoft.AspNetCore.Mvc;
using Tello_Demo.Application.DTOs;
using Tello_Demo.Application.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tello_Demo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet("{id}")]
    public async Task<CardDTO> Get(int id)
    {
        var result = await _cardService.GetCardByIdAsync(id);

        return result.Value;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CardDTO cardDTO)
    {
        var result = await _cardService.CreateCardAsync(cardDTO);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Errors);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleteCard = await _cardService.GetCardByIdAsync(id);
        var result = await _cardService.DeleteCardAsync(deleteCard.Value);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Errors);
    }
}
