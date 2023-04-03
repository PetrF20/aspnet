using InsuranceCorp.Data;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceCorp.MVC.Controllers
{
    public class StatusController : Controller
    {
        private readonly InsCorpDbContext _context; //můžeme použít pro všechny metody

        //Inject DB context
        public StatusController(InsCorpDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
