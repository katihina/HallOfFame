using AutoMapper;
using HallOfFame.Data;
using HallOfFame.Dtos;
using HallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Controllers
{
    [ApiController]
    [Route("api/v1/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly HallOfFameDbContext _context;
        private readonly IMapper _mapper;

        public PersonsController(HallOfFameDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
    // GET api/v1/persons
    [HttpGet]
    public async Task<ActionResult<List<PersonDto>>> GetAll()
    {
        var persons = await _context.Persons.Include(p => p.Skills).ToListAsync();
        var dtos = _mapper.Map<List<PersonDto>>(persons);
        return Ok(dtos);
    }
    
    // GET api/v1/persons/{id}
    [HttpGet("{id:long}")]
    public async Task<ActionResult<PersonDto>> Get(long id)
    {
        var person = await _context.Persons.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
        if (person == null)
            return NotFound();
        
        return Ok(_mapper.Map<PersonDto>(person));
    }
    
    // POST api/v1/persons
    [HttpPost]
    public async Task<ActionResult<PersonDto>> Create([FromBody] PersonCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var person = _mapper.Map<Person>(dto);

    foreach (var s in person.Skills)
    {
        s.Id = 0; 
    }
    
    _context.Persons.Add(person);
    await _context.SaveChangesAsync();
    var resultDto = _mapper.Map<PersonDto>(person);
    return Ok(resultDto);
    }

    // PUT api/v1/persons/{id}
    [HttpPut("{id:long}")]
    public async Task<ActionResult<PersonDto>> Update(long id, [FromBody] PersonCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var person = await _context.Persons.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
        if (person == null)
            return NotFound();
        
        person.Name = dto.Name;
        person.DisplayName = dto.DisplayName;
        
        var incoming = dto.Skills;
        
        var toRemove = person.Skills.Where(s => !incoming.Any(i => string.Equals(i.Name, s.Name, StringComparison.OrdinalIgnoreCase))).ToList();
        _context.Skills.RemoveRange(toRemove);
        
        foreach (var skillDto in incoming)
        {
            var existing = person.Skills.FirstOrDefault(s => string.Equals(s.Name, skillDto.Name, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.Level = skillDto.Level;
            }
            else
            {
                var newSkill = new Skill { Name = skillDto.Name, Level = skillDto.Level, Person = person };
                person.Skills.Add(newSkill);
            }
        }
        
        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<PersonDto>(person));
    }


    // DELETE api/v1/persons/{id}
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var person = await _context.Persons.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
        if (person == null) return NotFound();
        
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();

        return Ok();
    }
    }
}