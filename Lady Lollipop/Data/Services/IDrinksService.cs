using Lady_Lollipop.Data.Base;
using Lady_Lollipop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lady_Lollipop.Data.Services
{
    public interface IDrinksService : IEntityBaseRepository<Drink>
    {
        Task<Drink> GetDrinkByIdAsync(int id);
    }
}
