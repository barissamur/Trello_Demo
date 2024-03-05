using Microsoft.AspNetCore.Mvc;
using Tello_Demo.Application.DTOs;
using Tello_Demo.Application.Interfaces;
using Tello_Demo.Application.Services;

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


        // GET: api/<CardListController>
        [HttpGet]
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
        public async Task Post([FromBody] CardListDTO cardListDTO)
        {
            await _cardListService.CreateCardListAsync(cardListDTO);
        }

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
