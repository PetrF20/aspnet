using InsuranceCorp.Data;
using InsuranceCorp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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

		public IActionResult Detail(int id)
		{
			//1. získat data
			var person = _context.Persons.Find(id); //pokdu jde o primární klíč, tak je možné hledat přes Find

            /*vrátí stránka nenalezena
            if (person == null)
				return NotFound(); 
			*/

            //pokud nenajde osobu, zobrazíme nove View
            if (person == null)
			{
				ViewData["id"] = id;
				return View("NotFound");				
			}
                
			//2. zobrazit view
			return View(person);
		}

		public IActionResult Add() //metoda pro přidání nového prázdného formuláře
		{
			return View();
		}

		[HttpPost] //říká, že následující metoda bude volána jako http post
		public IActionResult Add(Person person)  //nutno přidat using
		{ 
			// uložit osobu do db
			_context.Persons.Add(person);
			var changed = _context.SaveChanges(); //uloží záznam do db

			if (changed > 0)
			{

				// kam přesměrujeme uživatele
				// metoda nevyrenderuje HTML, ale odkáže na jinou stránku
				return Redirect($"/person/detail/{person.Id}");

				//db po uložení vepíše ID zpět do person
				// dolar umožní poskladáat string pomocí složených závorek

				//alternativy
				//return RedirectToAction("Idnex");
			}            
			else
			{
                //neuloženo do DB...                
				return View("NotFound"); //nebo něco jiného
            }
			

        }

		public IActionResult Edit(int id)
		{
			// najít osobu z DB
			var person = _context.Persons.Find(id);
			//if (person == null) { } //ošetřit, pokud nenajdu, tak někam přesměrovat

			// zobrazit editační fomrulář
			return View(person);

		}

		[HttpPost]
        public IActionResult Edit(Person form_person) //hodnota, která přišla z formuláře, nejedná se o navázání na db
        {
            // najít osobu z DB
            var db_person = _context.Persons.Find(form_person.Id);

			// upravit hodnoty v DB (dle toho, co přišlo z formláře)
			db_person.FirstName = form_person.FirstName;
			db_person.LastName = form_person.LastName;
			db_person.Email = form_person.Email;	
			db_person.DateOfBirth = form_person.DateOfBirth;
			
			//uložit do DB
			_context.SaveChanges();


			//zobrazení - vrátíme info na stejnou stránku
			ViewData["succes_message"] = "Záznam úspěšně změněn a uložen v DB.";			
			return View(db_person);

		}



    }
}
