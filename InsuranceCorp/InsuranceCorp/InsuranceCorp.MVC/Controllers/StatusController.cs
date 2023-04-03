using InsuranceCorp.Data;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceCorp.MVC.Controllers
{
    public class StatusController : Controller
    {
        private readonly InsCorpDbContext _context; //můžeme použít pro všechny metody

        //Inject DB context
        public StatusController(InsCorpDbContext context) //konstruktor
        {
            _context = context;

        }

        public IActionResult Index()
        {
            bool ok = _context.Database.CanConnect(); //ověření  - dokáže se DB připojit??

            ViewData["ok"] = ok; //dictionary na klíči "ok" bude mít hodnotu z proměnné ok

            return View();
        }
    }
}
