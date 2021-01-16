using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bakery.Controllers
{
  public class HomeController : Controller
  {
    private readonly BakeryContext _db;
    public HomeController(BakeryContext db)
    {
      _db = db;
    }
    
    [HttpGet("/")]
    public ActionResult Index()
    {
      var treats = _db.Treats.ToList();
      var flavors = _db.Flavors.ToList();
      Dictionary<string, object> model = new Dictionary<string, object> {};
      model.Add("treat", treats);
      model.Add("flavor", flavors);
      return View(model);
    }
  }
}