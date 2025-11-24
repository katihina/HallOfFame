using System.ComponentModel.DataAnnotations;

namespace HallOfFame.Dtos
{
    public class SkillDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 10)]
        public byte Level { get; set; }
    }
}