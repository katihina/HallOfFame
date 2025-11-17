using HallOfFame.Models;
using HallOfFame.Services;
using Microsoft.AspNetCore.Mvc;

namespace HallOfFame.Controllers
{
    [Route("api/v1/[controller]")] 
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;
        
        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
        public async Task<ActionResult<IEnumerable<Person>>> Get()
        {
            var persons = await _personService.GetAllAsync();
            return Ok(persons);
        }


        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public async Task<ActionResult<Person>> GetById(long id)
        {
            var person = await _personService.GetByIdAsync(id);

            if (person == null)
            {
                return NotFound(); 
            }

            return Ok(person);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<ActionResult<Person>> Post([FromBody] Person person)
        {
            if (person.Id != 0)
            {
                return BadRequest("Id должен быть пустым при создании."); 
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var createdPerson = await _personService.CreateAsync(person);
            
            return CreatedAtAction(nameof(Get), new { id = createdPerson.Id }, createdPerson);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public async Task<ActionResult<Person>> Put(long id, [FromBody] Person person)
        {
            if (person.Id != 0)
            {
                return BadRequest("Id должен быть пустым в теле PUT-запроса."); 
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPerson = await _personService.UpdateAsync(id, person);

            if (updatedPerson == null)
            {
                return NotFound(); 
            }

            return Ok(updatedPerson); 
        }
        
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public async Task<IActionResult> Delete(long id)
        {
            var isDeleted = await _personService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound(); 
            }

            return NoContent();
        }
    }
}