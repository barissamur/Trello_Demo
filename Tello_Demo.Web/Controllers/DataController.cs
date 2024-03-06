﻿using Microsoft.AspNetCore.Mvc;
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


    [HttpPost("SetIndexCards")]
    public async Task<IActionResult> SetIndexCards([FromBody] List<SetIndex> setIndexs)
    { 
        return Ok();
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

    // POST: DataController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
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
