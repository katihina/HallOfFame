using AutoMapper;
using HallOfFame.Data;
using HallOfFame.Dtos;
using HallOfFame.Models;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Services
{
    public class PersonService : IPersonService
    {
        private readonly HallOfFameDbContext _context;
        private readonly IMapper _mapper;
        
        public PersonService(HallOfFameDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PersonDto>> GetAllAsync()
        {
            var persons = await _context.Persons.Include(p => p.Skills).ToListAsync();
            return _mapper.Map<List<PersonDto>>(persons);
        }

        public async Task<PersonDto?> GetByIdAsync(long id)
        {
            var person = await _context.Persons.Include(p => p.Skills)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (person == null) return null;
            
            return _mapper.Map<PersonDto>(person);
        }
        
        public async Task<PersonDto> CreateAsync(PersonCreateDto dto)
        {
            var person = _mapper.Map<Person>(dto);
            foreach (var skill in person.Skills)
                skill.Id = 0;

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return _mapper.Map<PersonDto>(person);
        }

        public async Task<PersonDto?> UpdateAsync(long id, PersonCreateDto dto)
        {
            var person = await _context.Persons.Include(p => p.Skills)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (person == null) return null;

            person.Name = dto.Name;
            person.DisplayName = dto.DisplayName;

            var toRemove = person.Skills
                .Where(s => !dto.Skills.Any(i => i.Name.Equals(s.Name, StringComparison.OrdinalIgnoreCase))).ToList();
            _context.Skills.RemoveRange(toRemove);
            foreach (var skillDto in dto.Skills)
            {
                var existing = person.Skills.FirstOrDefault(s => s.Name.Equals(skillDto.Name, StringComparison.OrdinalIgnoreCase));
                if (existing != null)
                    existing.Level = skillDto.Level;
                else
                    person.Skills.Add(new Skill { Name = skillDto.Name, Level = skillDto.Level, Person = person });
            }
            
            await _context.SaveChangesAsync();
            return _mapper.Map<PersonDto>(person);
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            var person = await _context.Persons.Include(p => p.Skills)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (person == null) return false;


            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}