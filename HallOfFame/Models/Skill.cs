using Newtonsoft.Json;

namespace HallOfFame.Models
{
    public class Skill
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; } 
        public long PersonId { get; set; }
        
        [JsonIgnore]
        public Person? Person { get; set; }
    }
}