using Lady_Lollipop.Data.Base;
using Lady_Lollipop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lady_Lollipop.Data.Services
{
    public class CakesService : EntityBaseRepository<Cake>, ICakesService
    {
        private readonly ApplicationDbContext _context;

        public CakesService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cake> GetCakeByIdAsync(int id)
        {
            var cakeDetails = await _context.Cakes.FirstOrDefaultAsync(n => n.Id == id);
            return cakeDetails;
        }
    }
}
