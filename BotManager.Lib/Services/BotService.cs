using BotManager.Lib.Data;
using BotManager.Lib.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BotManager.Lib.Services
{
    public interface IBotService
    {
        Task AddNewKc(string username, List<Loot> gatheredItems);
        Task<List<BotProfile>> GetAllBots();
        Task<BotProfile> GetProfile(string username);
        Task<int?> GetRemainingKcForUser(string name);

        /// <summary>
        /// Registers the bot to the database if it doesn't exist yet
        /// </summary>
        /// <param name="name">Bot username</param>        
        public Task<BotProfile> RegisterBot(string name);
    }

    public sealed class BotService : IBotService
    {
        private readonly ManagerContext _context;

        public BotService(ManagerContext context)
        {
            this._context = context;
        }

        public async Task AddNewKc(string username, List<Loot> lootItems)
        {
            var botProfile = await _context.BotProfile
                .Include(x => x.Activity)
                .ThenInclude(x => x.Loot)
                .Where(x => x.Name == username)
                .FirstOrDefaultAsync();

            if (botProfile is null)
            {
                return;
            }

            var activity = botProfile.Activity.Where(x => x.Date == DateTime.Today).FirstOrDefault();
            if (activity is null)
            {
                return;
            }
                        
            activity.KillsDone += 1;
            _context.BotActivity.Update(activity);
            await _context.SaveChangesAsync();

            foreach (var lootItem in lootItems)
            {
                lootItem.BotActivityId = activity.Id;
                var item = _context.Items.Where(x => x.RunescapeId == lootItem.RunescapeId).FirstOrDefault();
                lootItem.Item = item;
                _context.Loot.Add(lootItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<BotProfile>> GetAllBots()
        {
            return await _context.BotProfile
                .Include(x => x.Activity)
                .ThenInclude(x => x.Loot)
                .ThenInclude(x => x.Item)
                .ToListAsync();
        }

        public async Task<BotProfile> GetProfile(string username)
        {
            if(!_context.BotProfile.Any(x => x.Name.Equals(username)))
            {
                return null;
            }

            return await _context.BotProfile
                .Include(x => x.Activity)
                .ThenInclude(x => x.Loot)
                .ThenInclude(x => x.Item)
                .Where(x => x.Name == username)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// This should be done differently, but I CBA doing it nicely because this takes time.
        /// I know this implementation violates several coding standards but   ^^^^^^^^^^^^^^
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<int?> GetRemainingKcForUser(string username)
        {
            var botProfile = await _context.BotProfile
                .Include(x => x.Activity)
                .Where(x => x.Name == username)
                .FirstOrDefaultAsync();

            if (botProfile is null)
            {
                return null;
            }

            var activity = botProfile.Activity.Where(x => x.Date == DateTime.Today).FirstOrDefault();

            if (activity is null)
            {
                var currentSettings = _context.Settings.First() ?? new Settings();
                activity = new BotActivity()
                {
                    Date = DateTime.Today,
                    MaxKills = currentSettings.KillCountPerDay
                };

                botProfile.Activity.Add(activity);
                await _context.SaveChangesAsync();
            }

            int kcLeft = activity.MaxKills - activity.KillsDone;
            return KcGenerator.GetRandomKcCount(kcLeft);
        }

        public async Task<BotProfile> RegisterBot(string name)
        {
            var botProfile = _context.BotProfile.Include(x => x.Activity).Where(x => x.Name == name).FirstOrDefault();
            if (botProfile is null)
            {
                botProfile = new BotProfile()
                {
                    Name = name,
                };

                _context.BotProfile.Add(botProfile);
                await _context.SaveChangesAsync();
            }

            var botActivity = botProfile.Activity.Where(x => x.Date == DateTime.Today).FirstOrDefault();
            if (botActivity is null)
            {
                var currentSettings = _context.Settings.First() ?? new Settings();
                botActivity = new BotActivity()
                {
                    Date = DateTime.Today,
                    MaxKills = currentSettings.KillCountPerDay
                };

                botProfile.Activity.Add(botActivity);
                await _context.SaveChangesAsync();
            }

            return botProfile;
        }
    }
}
