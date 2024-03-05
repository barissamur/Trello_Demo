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
        private readonly ICardService _cardService;

        public CardListController(ICardListService cardListService
            , ICardService cardService)
        {
            _cardListService = cardListService;
            _cardService = cardService;
        }


        // GET: api/<CardListController> 
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CardListController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CardListController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CardListDTO cardListDTO)
        {
            var result = await _cardListService.CreateCardListAsync(cardListDTO);
            if (result.IsSuccess)
            {
                return Ok(result.Value); // veya CreatedAtRoute, CreatedAtAction vb.
            }

            return BadRequest(result.Errors);
        }


        // Oluşturulan CardList'i getiren bir aksiyon örneği. Gerçek uygulamanızda buna benzer bir aksiyonunuz olmalı


        // PUT api/<CardListController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CardListController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
