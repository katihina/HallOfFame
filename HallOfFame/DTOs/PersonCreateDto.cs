using System.ComponentModel.DataAnnotations;

namespace HallOfFame.Dtos
{
    public class PersonCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string DisplayName { get; set; } = string.Empty;
        
        [Required]
        public List<SkillDto> Skills { get; set; } = new();
    }
}