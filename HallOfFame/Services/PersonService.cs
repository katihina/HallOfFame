using HallOfFame.Data;
using HallOfFame.Models;
using HallOfFame.Services;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Services
{
    public class PersonService : IPersonService
    {
        private readonly HallOfFameDbContext _context;
        
        public PersonService(HallOfFameDbContext context)
        {
            _context = context;
        }

        public async Task<List<Person>> GetAllAsync()
        {
            return await _context.Persons
                .Include(p => p.Skills) 
                .ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(long id)
        {
            return await _context.Persons
                .Include(p => p.Skills)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<Person> CreateAsync(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            
            return person;
        }
        
        public async Task<Person?> UpdateAsync(long id, Person updatedData)
        {
            var existingPerson = await _context.Persons
                .Include(p => p.Skills)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPerson == null)
                return null;
            
            existingPerson.Name = updatedData.Name;
            existingPerson.DisplayName = updatedData.DisplayName;
            
            var newSkills = updatedData.Skills ?? new List<Skill>();
            var oldSkills = existingPerson.Skills;
            
            var skillsToRemove = oldSkills
                .Where(os => newSkills.All(ns => ns.Name != os.Name))
                .ToList();

            foreach (var skill in skillsToRemove)
            {
                _context.Skills.Remove(skill);
            }
            
            foreach (var newSkill in newSkills)
            {
                var existingSkill = oldSkills
                    .FirstOrDefault(os => os.Name == newSkill.Name);

                if (existingSkill != null)
                {
                    existingSkill.Level = newSkill.Level;
                }
                else
                {
                    newSkill.PersonId = id;
                    _context.Skills.Add(newSkill);
                }
            }

            await _context.SaveChangesAsync();

            return existingPerson;
        }

        
        public async Task<bool> DeleteAsync(long id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return false; 
            }

            _context.Persons.Remove(person);
            
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}