using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BotManager.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BotManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemUpdateController : ControllerBase
    {
        private readonly ManagerContext _context;

        public ItemUpdateController(ManagerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateItemsAsync()
        {
            var uri = "https://prices.runescape.wiki/api/v1/osrs/mapping";

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("User-Agent", "BotManager Item update tool");

            var httpResult = await new HttpClient().SendAsync(request);

            var jsonContent = await httpResult.Content.ReadAsStringAsync();

            var parsedContent = JsonConvert.DeserializeObject<dynamic>(jsonContent);

            foreach (dynamic item in parsedContent)
            {
                var id = (string)item["id"];
                var name = (string)item["name"];

                await _context.Items.AddAsync(new Data.Entities.Item()
                {
                    Name = name,
                    RunescapeId = int.Parse(id)
                });

            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var botProfile = await _context.BotProfile
                .Include(x => x.Activity)
                .ThenInclude(x => x.Loot)
                .ThenInclude(x => x.Item)
                .FirstOrDefaultAsync();

            var activity = botProfile.Activity.First();

            var sorted = activity.Loot
                .ToList();

            return Ok();
        }
    }
}
