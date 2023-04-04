using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceCorp.Data;
using InsuranceCorp.Model;

namespace InsuranceCorp.API.Controllers
{
    [Route("api/[controller]")] //anotace pro cestu, za doménou v URL bude API/person
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly InsCorpDbContext _context;

        public PersonController(InsCorpDbContext context) //pomocí injection vložil kontext (db)
        {
            _context = context;
        }

        // GET: api/Person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons() //asynchronní volání na webu - využití při hodně requestů současně, pro běžné použití nevyužito
        {
          if (_context.Persons == null) //vrátí všechny osoby (tabulku do listu)
          {
              return NotFound();
          }
            // return await _context.Persons.Take(50).ToListAsync();

            return await _context.Persons
                .Include(person => person.Address)
                .Include(person => person.Contracts)
                .Take(50).ToListAsync();

        }

        // GET: api/Person/5
        [HttpGet("{id}")]       //skládá url za sebe po lomítkách
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
          if (_context.Persons == null)
          {
              return NotFound();
          }
            var person = await _context.Persons
                .Include(person => person.Address) //zřetězení tabulky address (přidání, aby byla taky ve výstupu)
                .Include(person => person.Contracts)
                .FirstOrDefaultAsync(person => person.Id == id);
                //.FindAsync(id); //metoda find se nedá použít, pokud používáme include

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/Person/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // použití pro editaci ID, druhým parametrem je sada Person

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person) 
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Person
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //vytvoření osoby - alternativa metody Add v projektu MVC
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
          if (_context.Persons == null)
          {
              return Problem("Entity set 'InsCorpDbContext.Persons'  is null.");
          }
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            if (_context.Persons == null)
            {
                return NotFound();
            }
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Person/5
        [HttpGet("email/{email}")]       //skládá url za sebe po lomítkách
        public async Task<ActionResult<Person>> GetPersonByEmail(string email)
        {
            if (_context.Persons == null)
            {
                return NotFound();
            }
            var person = await _context.Persons
                .Where(person => person.Email.ToUpper() == email.ToUpper())                
                .FirstOrDefaultAsync();            

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }


        // GET: api/Person/5
        [HttpGet("city/{city}")]       //skládá url za sebe po lomítkách
        public async Task<ActionResult> GetPersonByCity(string city)        
        {
            if (_context.Persons == null)
            {
                return NotFound();
            }
            return Ok(_context.Persons
                .Include(person => person.Address)
                .Include(person => person.Contracts)
                //.Where(person => person.Address.City.ToUpper() == city.ToUpper())
                .Where(person => person.Address != null && person.Address.City.ToUpper() == city.ToUpper())
                .Select(person => new {person.FirstName, person.LastName, person.Address.City })            
                .ToList());

                //.select / new - vytvoření nového anonymního typu - je vytvořený jen pro toto místo, v "select" je možné zavolat i nějakou metodu
        }


        private bool PersonExists(int id)
        {
            return (_context.Persons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
