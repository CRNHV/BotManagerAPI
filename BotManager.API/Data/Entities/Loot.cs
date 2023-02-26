using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotManager.Api.Data.Entities
{
    public class Loot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        [Column]
        [ForeignKey("Item")]

        public int RunescapeId { get; set; }

        [Column]
        public int Quantity { get; set; }

        public int BotActivityId { get; set; }

        [ForeignKey("RunescapeId")]
        public virtual Item Item { get; set; }
    }
}
