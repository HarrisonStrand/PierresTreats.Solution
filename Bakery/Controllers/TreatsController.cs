using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System;

namespace Bakery.Controllers
{
  public class TreatsController : Controller
  {
    private readonly BakeryContext _db;
    public TreatsController(BakeryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List <Treat> model = _db.Treats.ToList();
      return View(model);
    }
    public ActionResult BestOf()
    {
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "TreatName", "StarRating");
      return View(_db.Flavors.OrderByDescending(m=>m.StarRating).ToList());
    }

    public async Task<IActionResult> SearchBy(string searchString)
    {
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "TreatName", "Ingredients");
      var search = from m in _db.Treats
        select m;

      if (!String.IsNullOrEmpty(searchString))
      {
          search = search.Where(s => s.Ingredients.Contains(searchString));
      }
      return View(await search.ToAsyncEnumerable().ToList()); // This line is different and does not require any additional using directives or packages to use
    }
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Treat treat)
    {
      _db.Treats.Add(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisTreat = _db.Treats
        .Include(Treat => Treat.JoinEntries)
        .ThenInclude(join => join.Flavor)
        .FirstOrDefault(Treat => Treat.TreatId == id);
      return View(thisTreat);
    }

    public ActionResult Edit(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(Treat => Treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddFlavor(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "FlavorName");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddFlavor(Flavor flavor, int TreatId)
    {
      if (TreatId != 0)
      {
        var returnedJoin = _db.FlavorTreat.Any(join => join.FlavorId == flavor.FlavorId && join.TreatId == TreatId); 
        if (!returnedJoin) 
        {
          _db.FlavorTreat.Add(new FlavorTreat() { TreatId = TreatId, FlavorId = flavor.FlavorId });
        }
      }
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
    
  }
}