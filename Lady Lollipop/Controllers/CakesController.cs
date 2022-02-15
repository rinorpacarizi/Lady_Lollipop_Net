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

        public CakesController(ICakesService service)
        {
            _service = service;
        }

        // GET: Cakes
        public async Task<IActionResult> Index(int page=1)
        {
            List<Cake> cakes = (List<Cake>)await _service.GetAllAsync();
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
