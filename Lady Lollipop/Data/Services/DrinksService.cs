using Lady_Lollipop.Data.Base;
using Lady_Lollipop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lady_Lollipop.Data.Services
{
    public class DrinksService : EntityBaseRepository<Drink>, IDrinksService
    {
        private readonly ApplicationDbContext _context;

        public DrinksService(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<Drink> GetDrinkByIdAsync(int id)
        {
            var drinksDetails = await _context.Drinks.FirstOrDefaultAsync(n => n.Id == id);
            return drinksDetails;
        }
    }
}
