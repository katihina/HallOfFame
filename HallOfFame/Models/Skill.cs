using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HallOfFame.Models
{
    public class Skill
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 10)]
        public byte Level { get; set; }
        
        public long PersonId { get; set; }
        public Person? Person { get; set; }
    }
}