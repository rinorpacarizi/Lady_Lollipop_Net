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
    public class SweetsController : Controller
    {
        private readonly ISweetsService _service;
        private readonly ApplicationDbContext _context;

        public SweetsController(ISweetsService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        // GET: Sweets
        public async Task<IActionResult> Index(string sortOrder,int page=1)
        {
            ViewBag.PriceSortParam =String.IsNullOrEmpty(sortOrder)? "price_desc": "";
            ViewBag.StockSortParam = String.IsNullOrEmpty(sortOrder) ? "stock_desc" : "stock_asc";
            var sweets = await _context.Sweets.ToListAsync();
            sweets = sweets.OrderByDescending(s => s.Price).ToList<Sweet>();

            switch (sortOrder)
            {
                case "price_desc":
                   sweets = sweets.OrderBy(s => s.Price).ToList<Sweet>();
                    break;
                case "stock_desc":
                    sweets = sweets.OrderByDescending(s => s.Stock).ToList<Sweet>();
                    break;
                case "stock_asc":
                    sweets = sweets.OrderBy(s => s.Stock).ToList<Sweet>();
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
            int resCount = sweets.Count();
            var pager = new Pagination(resCount, page, pageSize);
            int resSkip = (page - 1) * pageSize;
            var data = sweets.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pagination = pager;
            return View(data);
        }
        public async Task<IActionResult> Filter(string searchString)
        {
            var allSweets = await _service.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allSweets.Where(n => n.Name.Contains(searchString)).ToList();
                return View("Index", filteredResult);
            }

            return View("Index", allSweets);
        }

        // GET: Sweets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var sweet = await _service.GetSweetByIdAsync(id);
                
            if (sweet == null)
            {
                return NotFound();
            }

            return View(sweet);
        }

        // GET: Sweets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sweets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Picture,Name,Price,Stock,Description,Ingridients")] Sweet sweet)
        {
            if (!ModelState.IsValid)
            {
                return View(sweet);
            }
            await _service.AddAsync(sweet);
            return RedirectToAction(nameof(Index));
        }

        // GET: Sweets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var sweet = await _service.GetByIdAsync(id);
            if (sweet == null) return NotFound();
            return View(sweet);
        }

        // POST: Sweets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Picture,Name,Price,Stock,Description,Ingridients")] Sweet sweet)
        {
            if (id != sweet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(id,sweet);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SweetExists(sweet.Id))
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
            return View(sweet);
        }

        // GET: Sweets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var sweet = await _service.GetByIdAsync(id);
            if (sweet == null) return NotFound();
            return View(sweet);
        }

        // POST: Sweets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sweet = await _service.GetByIdAsync(id);
            if (sweet == null) return NotFound();
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool SweetExists(int id)
        {
            // return _context.Sweets.Any(e => e.Id == id);
            return _service.Any(id);
        }
    }
}
