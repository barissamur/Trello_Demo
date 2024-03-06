using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tello_Demo.Application.DTOs;
using Tello_Demo.Application.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tello_Demo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardListController : ControllerBase
    {
        private readonly ICardListService _cardListService;

        public CardListController(ICardListService cardListService)
        {
            _cardListService = cardListService;
        }


        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<CardListDTO>> Get()
        {
            var result = await _cardListService.GetCardListAsync();

            return result.Value;
        }

        // GET api/<CardListController>/5
        [HttpGet("{id}")]
        public async Task<CardListDTO> Get(int id)
        {
            var result = await _cardListService.GetCardListByIdAsync(id);

            return result.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CardListDTO cardListDTO)
        {
            var result = await _cardListService.CreateCardListAsync(cardListDTO);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CardListDTO cardListDTO)
        {
            var result = await _cardListService.UpdateCardListAsync(cardListDTO);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleteCardList = await _cardListService.GetCardListByIdAsync(id);
            var result = await _cardListService.DeleteCardListAsync(deleteCardList.Value);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Errors);
        }
    }
}
