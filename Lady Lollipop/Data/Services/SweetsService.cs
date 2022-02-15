using Lady_Lollipop.Data.Base;
using Lady_Lollipop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lady_Lollipop.Data.Services
{
    public class SweetsService:EntityBaseRepository<Sweet>,ISweetsService
    {
        private readonly ApplicationDbContext _context; 
        public SweetsService(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<Sweet> GetSweetByIdAsync(int id)
        {
            var sweetsDetails = await _context.Sweets.FirstOrDefaultAsync(n => n.Id == id);
            return sweetsDetails;
        }
    }
}
