using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotManager.Api.Data.Entities
{
    public class BotActivity
    {
        [Column]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column]
        public DateTime Date { get; set; }

        [Column]
        public int KillsDone { get; set; } = 0;

        [Column]
        public int MaxKills { get; set; }

        [Column]
        [ForeignKey("BotActivityId")]
        public ICollection<Loot> Loot { get; set; } = new List<Loot>();

    }
}
