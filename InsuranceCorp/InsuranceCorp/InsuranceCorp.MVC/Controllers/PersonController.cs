using InsuranceCorp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCorp.MVC.Controllers
{
    public class PersonController : Controller
    {
        private readonly InsCorpDbContext _context;

        //inject dbcontext session
        //zkratka "ctor" - vytvoření construktoru
        public PersonController(InsCorpDbContext context)
        {
            _context = context;            
        }

		public IActionResult Index()
        {
            var top100 = _context.Persons
			    .Include(person => person.Constracts)
			    .OrderByDescending(person => person.Constracts.Count())
			    .Take(10).ToList();

			return View(top100);
        }

		public IActionResult Osoby()
		{
			//1. ztískat data - před tím než vrátí view získáme seznam prvních 100 osob 
			//_context.Persons říká, že pracuje jen s touto tabulkou,p okud chci pracovat i s jinou tabulkou, připojím pomocí include
			var top100 = _context.Persons
                .Include(person => person.Constracts) //připojí tabulku kontraktů
				//.OrderBy(person => person.Id)
                .OrderByDescending(person => person.Constracts.Count())
				.Take(10).ToList();

			/*nebo lze rozdělit
            var query = _context.Persons
                .OrderBy(person => person.Id);
            var top100 = query.Take(100);
            */

			//2. zobrzait výsledek uživateli
			return View(top100);
		}


		public IActionResult Detail(int idOsoby)
		{
			//1. získat data
			var person = _context.Persons.Find(idOsoby); //pokdu jde o primární klíč, tak je možné hledat přes Find

			//2. zobrazit view
			return View(person);
		}

	}
}
