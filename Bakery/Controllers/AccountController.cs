using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Bakery.Models;
using System.Threading.Tasks;
using Bakery.ViewModels;
using System.Linq;
using System.Collections.Generic;

namespace Bakery.Controllers
{
  public class AccountController : Controller
  {
    private readonly BakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, BakeryContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }

    public ActionResult Index()
    {
      var treats = _db.Treats.ToList();
      var flavors = _db.Flavors.ToList();
      Dictionary<string, object> model = new Dictionary<string, object> {};
      model.Add("treat", treats);
      model.Add("flavor", flavors);
      return View(model);
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register (RegisterViewModel model)
    {
      var user = new ApplicationUser { UserName = model.UserName };
      IdentityResult result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        return RedirectToAction("Index", "Account");
      }
      else
      {
        return View("UnsuccessfulRegister");
      }
    }
    public ActionResult Login()
    {
      return View();  
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return View("UnsuccessfulLogin");
      }
    }
    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }
  }
}