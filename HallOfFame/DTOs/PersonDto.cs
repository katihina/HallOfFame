namespace HallOfFame.Dtos
{
    public class PersonDto
    {
        public long Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string DisplayName { get; set; } = string.Empty;
        
        public List<SkillDto> Skills { get; set; } = new();
    }
}