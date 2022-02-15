using Lady_Lollipop.Data.Base;
using Lady_Lollipop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lady_Lollipop.Data.Services
{
    public interface ISweetsService : IEntityBaseRepository<Sweet>
    {
        Task<Sweet> GetSweetByIdAsync(int id);
    }
}
