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
    public class CakesController : Controller
    {
        private readonly ICakesService _service;
        private readonly ApplicationDbContext _context;

        public CakesController(ICakesService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        // GET: Cakes
        public async Task<IActionResult> Index(string sortOrder, int page=1)
        {
            ViewBag.PriceSortParam = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.StockSortParam = String.IsNullOrEmpty(sortOrder) ? "stock_desc" : "stock_asc";
            var cakes = await _context.Cakes.ToListAsync();
            cakes = cakes.OrderByDescending(s => s.Price).ToList<Cake>();

            switch (sortOrder)
            {
                case "price_desc":
                    cakes = cakes.OrderBy(s => s.Price).ToList<Cake>();
                    break;
                case "stock_desc":
                    cakes = cakes.OrderByDescending(s => s.Stock).ToList<Cake>();
                    break;
                case "stock_asc":
                    cakes = cakes.OrderBy(s => s.Stock).ToList<Cake>();
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
            int resCount = cakes.Count();
            var pager = new Pagination(resCount, page, pageSize);
            int resSkip = (page - 1) * pageSize;
            var data = cakes.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pagination = pager;


            return View(data);
        }
        public async Task<IActionResult> Filter(string searchString)
        {
            var allCakes = await _service.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allCakes.Where(n => n.Name.Contains(searchString)).ToList();
                return View("Index", filteredResult);
            }

            return View("Index", allCakes);
        }

        // GET: Cakes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cake = await _service.GetCakeByIdAsync(id);

            if (cake == null)
            {
                return NotFound();
            }

            return View(cake);
        }

        // GET: Cakes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cakes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Picture,Name,Price,Stock,Description,Ingridients")] Cake cake)
        {
            if (!ModelState.IsValid)
            {
                return View(cake);
            }
            await _service.AddAsync(cake);
            return RedirectToAction(nameof(Index));
        }

        // GET: Cakes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var cake = await _service.GetByIdAsync(id);
            if (cake == null) return NotFound();
            return View(cake);
        }

        // POST: Cakes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Picture,Name,Price,Stock,Description,Ingridients")] Cake cake)
        {
            if (id != cake.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(id, cake);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CakeExists(cake.Id))
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
            return View(cake);
        }

        // GET: Cakes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cake = await _service.GetByIdAsync(id);
            if (cake == null) return NotFound();
            return View(cake);
        }

        // POST: Cakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cake = await _service.GetByIdAsync(id);
            if (cake == null) return NotFound();
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CakeExists(int id)
        {
            return _service.Any(id);
        }
    }
}
