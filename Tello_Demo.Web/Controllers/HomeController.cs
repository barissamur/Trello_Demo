using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tello_Demo.Web.Models;
using Tello_Demo.Web.Services;

namespace Tello_Demo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CardListService _cardListService;

        public HomeController(ILogger<HomeController> logger
            , CardListService cardListService)
        {
            _logger = logger;
            _cardListService = cardListService;
        }

        public async Task<IActionResult> Index()
        {
            var cardLists = await _cardListService.GetCardListsAsync();

            IndexVM model = new()
            {
                cardLists = cardLists
            };

            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
