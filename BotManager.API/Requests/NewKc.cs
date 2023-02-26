using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BotManager.Api.Requests
{
    public class NewKcRequest
    {
        [Required]
        public string Username { get; set; }

        public ICollection<Item> Items { get; set; }

    }

    public class Item
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
