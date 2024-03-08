using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tello_Demo.Web.Models;
using Tello_Demo.Web.Services;

namespace Tello_Demo.Web.Controllers;

[Route("api/[controller]")]
public class DataController : Controller
{
    private readonly CardListService _cardListService;
    private readonly CardService _cardService;
    private readonly CardLogService _cardLogService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DataController(CardListService cardListService
        , CardService cardService
        , CardLogService cardLogService
        , IWebHostEnvironment webHostEnvironment)
    {
        _cardListService = cardListService;
        _cardService = cardService;
        _cardLogService = cardLogService;
        _webHostEnvironment = webHostEnvironment;
    }


    [HttpGet("GetCardList")]
    public async Task<IActionResult> GetCardList()
    {
        var cardLists = await _cardListService.GetCardListsAsync();

        return Ok(cardLists);
    }


    [HttpPost("UpdateCardListAndCards")]
    public async Task<IActionResult> UpdateCardListAndCards([FromBody] List<CardList> cardLists, int cardId, int listId, string eventName)
    {
        var response = await _cardListService.UpdateCardListAsync(cardLists);

        var result = cardLists.SelectMany(cl => cl.Cards, (cl, card) => new { cl.Id, cl.Title, Card = card })
                              .FirstOrDefault(x => x.Card.Id == cardId);
        string details = "";

        if (cardId != 0)
        {

            details = cardLists.Count == 2
                ? $"Card Id: {cardId}, Card Title: {result.Card.Title} => {result.Title} listesine {eventName}"
                : $"Card Id: {cardId}, Card Title: {result.Card.Title} => {result.Title} listesi içinde {eventName}";

            await _cardLogService.LogCardEventAsync(details);
        }

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
    public async Task<IActionResult> DeleteCard(int id, string cardTitle)
    {
        try
        {
            var response = await _cardService.DeleteCardAsync(id);

            string details = $"{cardTitle} kartı silindi";

            await _cardLogService.LogCardEventAsync(details);

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
            card.Description = "Description";
            var response = await _cardService.CreateCardAsync(card);

            string details = $"{card.Title} kartı oluşturuldu";

            await _cardLogService.LogCardEventAsync(details);

            return Ok();

        }
        catch
        {
            return BadRequest();
        }
    }


    [HttpGet("GetLogData")]
    public async Task<IActionResult> GetLogData()
    {
        var logFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "log", "log", "CardOperationsLog.log");
        if (!System.IO.File.Exists(logFilePath))
        {
            return NotFound("Log dosyası bulunamadı.");
        }

        var logFileContent = await System.IO.File.ReadAllTextAsync(logFilePath);
         
        logFileContent = logFileContent.TrimEnd(','); 
        logFileContent = "[" + logFileContent + "]";

        var logEntries = JsonConvert.DeserializeObject<IEnumerable<CardLog>>(logFileContent);
        return Ok(logEntries);
    }




}