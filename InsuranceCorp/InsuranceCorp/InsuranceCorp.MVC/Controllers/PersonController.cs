using InsuranceCorp.Data;
using Microsoft.AspNetCore.Mvc;

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
            //1. ztískat data - před tím než vrátí view získáme seznam prvních 100 osob 
            var top100 = _context.Persons
                .OrderBy(person => person.Id)
                .Take(100).ToList();

            /*nebo lze rozdělit
            var query = _context.Persons
                .OrderBy(person => person.Id);
            var top100 = query.Take(100);
            */

            //2. zobrzait výsledek uživateli
            return View(top100);
        }
    }
}
