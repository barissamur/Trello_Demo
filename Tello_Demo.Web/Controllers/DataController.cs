using Microsoft.AspNetCore.Mvc;
using Tello_Demo.Web.Models;
using Tello_Demo.Web.Services;

namespace Tello_Demo.Web.Controllers;

[Route("api/[controller]")]
public class DataController : Controller
{
    private readonly CardListService _cardListService;
    private readonly CardService _cardService;

    public DataController(CardListService cardListService
        , CardService cardService)
    {
        _cardListService = cardListService;
        _cardService = cardService;
    }


    [HttpGet("GetCardList")]
    public async Task<IActionResult> GetCardList()
    {
        var cardLists = await _cardListService.GetCardListsAsync();

        return Ok(cardLists);
    }


    [HttpPost("UpdateCardListAndCards")]
    public async Task<IActionResult> UpdateCardListAndCards([FromBody] List<CardList> cardLists, bool onlyTitle)
    {
        var response = await _cardListService.UpdateCardListAsync(cardLists);
        return Ok();
    }


    [HttpPost("CreateList")]
    public async Task<IActionResult> CreateList([FromBody] CardList cardList)
    {
        try
        {
            var response = await _cardListService.CreateCardListAsync(cardList);

            return Ok(response);
        }
        catch
        {
            return BadRequest();
        }
    }


    [HttpDelete("DeleteList/{id}")]
    public async Task<IActionResult> DeleteList(int id)
    {
        try
        {
            var response = await _cardListService.DeleteCardListAsync(id);
            return Ok();

        }
        catch
        {
            return BadRequest();
        }
    }


    [HttpDelete("DeleteCard/{id}")]
    public async Task<IActionResult> DeleteCard(int id)
    {
        try
        {
            var response = await _cardService.DeleteCardAsync(id);
            return Ok();

        }
        catch
        {
            return BadRequest();
        }
    }


    [HttpPost("AddCardToList/{id}")]
    public async Task<IActionResult> AddCardToList([FromBody] Card card, int id)
    {
        try
        {
            card.CardList.Id = id;
            card.CardList.Type = "Type";
            card.Type = "Type";
            card.Description = "Açıklama";
            var response = await _cardService.CreateCardAsync(card);
            return Ok();

        }
        catch
        {
            return BadRequest();
        }
    }



















    // GET: DataController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: DataController/Create
    public ActionResult Create()
    {
        return View();
    }


    // GET: DataController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: DataController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: DataController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: DataController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
