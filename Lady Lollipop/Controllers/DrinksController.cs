using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lady_Lollipop.Data;
using Lady_Lollipop.Models;
using Lady_Lollipop.Data.Services;

namespace Lady_Lollipop.Controllers
{
    public class DrinksController : Controller
    {
        private readonly IDrinksService _service;
        private readonly ApplicationDbContext _context;

        public DrinksController(IDrinksService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        // GET: Drinks
        public async Task<IActionResult> Index(string sortOrder, int page=1)
        {
            ViewBag.PriceSortParam = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.StockSortParam = String.IsNullOrEmpty(sortOrder) ? "stock_desc" : "stock_asc";
            var drinks = await _context.Drinks.ToListAsync();
            drinks = drinks.OrderByDescending(s => s.Price).ToList<Drink>();

            switch (sortOrder)
            {
                case "price_desc":
                    drinks = drinks.OrderBy(s => s.Price).ToList<Drink>();
                    break;
                case "stock_desc":
                    drinks = drinks.OrderByDescending(s => s.Stock).ToList<Drink>();
                    break;
                case "stock_asc":
                    drinks = drinks.OrderBy(s => s.Stock).ToList<Drink>();
                    break;
                default:
                    //sweets =sweets.OrderBy(s => s.Price).ToList<Sweet>();
                    break;
            }

            const int pageSize = 8;
            if (page < 1)
            {
                page = 1;
            }
            int resCount = drinks.Count();
            var pager = new Pagination(resCount, page, pageSize);
            int resSkip = (page - 1) * pageSize;
            var data = drinks.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pagination = pager;


            return View(data);
        }
        public async Task<IActionResult> Filter(string searchString)
        {
            var allDrinks = await _service.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allDrinks.Where(n => n.Name.Contains(searchString)).ToList();
                return View("Index", filteredResult);
            }

            return View("Index", allDrinks);
        }

        // GET: Drinks/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var drink = await _service.GetDrinkByIdAsync(id);

            if (drink == null)
            {
                return NotFound();
            }

            return View(drink);
        }

        // GET: Drinks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Picture,Name,Price,Stock,Description,Ingridients")] Drink drink)
        {
            if (!ModelState.IsValid)
            {
                return View(drink);
            }
            await _service.AddAsync(drink);
            return RedirectToAction(nameof(Index));
        }

        // GET: Drinks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var drink = await _service.GetByIdAsync(id);
            if (drink == null) return NotFound();
            return View(drink);
        }

        // POST: Drinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Picture,Name,Price,Stock,Description,Ingridients")] Drink drink)
        {

            if (id != drink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(id, drink);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrinkExists(drink.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(drink);
        }

        // GET: Drinks/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var drink = await _service.GetByIdAsync(id);
            if (drink == null) return NotFound();
            return View(drink);
        }

        // POST: Drinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drink = await _service.GetByIdAsync(id);
            if (drink == null) return NotFound();
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool DrinkExists(int id)
        {
            return _service.Any(id);
        }
    }
}
