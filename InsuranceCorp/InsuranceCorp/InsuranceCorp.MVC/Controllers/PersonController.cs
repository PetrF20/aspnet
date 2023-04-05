using InsuranceCorp.Data;
using InsuranceCorp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
			    .Include(person => person.Contracts)
			    .OrderByDescending(person => person.Contracts.Count())
			    .Take(10).ToList();

			return View(top100);
        }

		public IActionResult Osoby()
		{
			//1. ztískat data - před tím než vrátí view získáme seznam prvních 100 osob 
			//_context.Persons říká, že pracuje jen s touto tabulkou,p okud chci pracovat i s jinou tabulkou, připojím pomocí include
			var top100 = _context.Persons
                .Include(person => person.Contracts) //připojí tabulku kontraktů
				//.OrderBy(person => person.Id)
                .OrderByDescending(person => person.Contracts.Count())
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
        
		[Authorize]
        public IActionResult Add() //metoda pro přidání nového prázdného formuláře
		{
			//ověření pžihlášeného uživatele User.Identity.Name == "fabo@seznam.cz"
			//role role based authorization
			//[Authorize]
			//net core itentity user manager
			//AddClaim
			//[Authorize RoleManager= ""]
			return View();
		}

		[HttpPost] //říká, že následující metoda bude volána jako http post
        [Authorize]
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

        [Authorize]
        public IActionResult Edit(int id)
		{
			// najít osobu z DB
			var person = _context.Persons.Find(id);
			//if (person == null) { } //ošetřit, pokud nenajdu, tak někam přesměrovat

			// zobrazit editační fomrulář
			return View(person);

		}

		[HttpPost]
		[Authorize] //povolí tuto metoidu /stránku jen pro přihlášené
        public IActionResult Edit(Person form_person) //hodnota, která přišla z formuláře, nejedná se o navázání na db
        {
			if (!ModelState.IsValid)    //kontrola/validace modelu, jestli jsou zadaná data správná - výsledek validace
            {
                /*např.
					ViewData["succes_message"] = "Záznam úspěšně změněn a uložen v DB.";			
					return View(db_person);

					nebo 
				ModelState[0].Errors[0].ErrorMessage;
				*/
            }

            // najít osobu z DB
            var db_person = _context.Persons.Find(form_person.Id);

            // upravit hodnoty v DB (dle toho, co přišlo z formláře)
            //1.možnost zápisu            
            db_person.FirstName = form_person.FirstName;
			db_person.LastName = form_person.LastName;
			db_person.Email = form_person.Email;	
			db_person.DateOfBirth = form_person.DateOfBirth;

            //2. možnost zápisu - doporučeno pokud chci přepsat všechny hodnoty, jinak varianta 1
            //tento příkaz nahradí ruční přiřazení řádků výše (db_person.FirstName = form_person.FirstName; atp.)
			// přepisuje jen ty property, které mu dojdou z formuláře
            //_context.Entry(db_person).CurrentValues.SetValues(form_person); 

            //3. možnost zápisu - nahradí přepsání z formuláře do db
            //_context.Entry(form_person).State = EntityState.Modified;

            //uložit do DB
            _context.SaveChanges();


			//zobrazení - vrátíme info na stejnou stránku
			ViewData["succes_message"] = "Záznam úspěšně změněn a uložen v DB.";			
			return View(db_person);

		}

		public IActionResult GetByEmail(string email) //volá se v url s otazníkem
		{
            //1. získat / vyheldat data - nelze použít Find
            //firstordefualt vrací null v případě, že nic nenajde
            var person = _context.Persons
				.Where(person => person.Email.ToUpper() == email.ToUpper())
				.FirstOrDefault();

			if (person == null)
				return NotFound();

			return View("Detail", person);
            

        }


    }
}
