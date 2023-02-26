using System.Collections.Generic;
using System.Threading.Tasks;
using BotManager.Lib.Data.Entities;
using BotManager.Lib.Services;
using Microsoft.AspNetCore.Mvc;

namespace BotManager.Api.Controllers
{

    [Route("/botview")]
    public class BotViewController : Controller
    {
        private readonly IBotService _botService;

        public BotViewController(IBotService botService)
        {
            _botService = botService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<BotProfile> allBots = await _botService.GetAllBots();

            return View(allBots);
        }

        [HttpGet("/bot/{username}")]
        public async Task<IActionResult> ViewBot([FromRoute] string username)
        {
            BotProfile botProfile = await _botService.GetProfile(username);

            return View(botProfile);
        }
    }
}
