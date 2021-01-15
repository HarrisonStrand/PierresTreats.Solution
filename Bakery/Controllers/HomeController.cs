using Microsoft.AspNetCore.Mvc;

namespace RecipeBook.Controllers
{
  public class HomeController : Controller
  {
    
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
  }
}