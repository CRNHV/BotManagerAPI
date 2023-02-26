using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BotManager.Api.Data.Entities
{
    public class BotProfile
    {

        [Column]
        [Key]
        public string Name { get; set; } = string.Empty;


        public ICollection<BotActivity> Activity { get; set; } = new List<BotActivity>();
    }
}
