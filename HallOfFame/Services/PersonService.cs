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
        
        public async Task<Person?> UpdateAsync(long id, Person updatedPersonData)
        {
            var existingPerson = await _context.Persons
                .Include(p => p.Skills)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPerson == null)
            {
                return null; 
            }

            existingPerson.Name = updatedPersonData.Name;
            existingPerson.DisplayName = updatedPersonData.DisplayName;

            _context.Skills.RemoveRange(existingPerson.Skills);

            foreach (var skill in updatedPersonData.Skills)
            {
                skill.PersonId = id; 
            }

            existingPerson.Skills = updatedPersonData.Skills;
            
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