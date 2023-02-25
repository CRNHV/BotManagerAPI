using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotManager.Api.Requests;
using BotManager.Lib.Services;
using Microsoft.AspNetCore.Mvc;

namespace BotManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        IBotService _botService;

        public BotController(IBotService botRegisterService)
        {
            _botService = botRegisterService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BotRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username))
                {
                    return BadRequest();
                }

                await _botService.RegisterBot(request.Username);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("remaining")]
        public async Task<IActionResult> GetRemaining([FromBody] BotRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username))
                {
                    return BadRequest();
                }

                int? remainingKc = await _botService.GetRemainingKcForUser(request.Username);
                if (remainingKc == null)
                {
                    return BadRequest();
                }

                return Ok(remainingKc);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("kc")]
        public async Task<IActionResult> AddKc([FromBody] NewKcRequest request)
        {
            try
            {
                List<Lib.Data.Entities.Loot> gatheredItems = new();

                foreach (var item in request.Items)
                {
                    gatheredItems.Add(new Lib.Data.Entities.Loot()
                    {
                        RunescapeId = item.ItemId,
                        Quantity = item.Quantity,
                    });
                }

                await _botService.AddNewKc(request.Username, gatheredItems);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetBotProfile([FromQuery] string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return BadRequest();
                }

                var botProfile = await _botService.GetProfile(username);
                if (botProfile == null)
                {
                    return BadRequest();
                }
                return Ok(botProfile);
            }
            catch (Exception ex)
            {
            }

            return Ok();
        }
    }
}
