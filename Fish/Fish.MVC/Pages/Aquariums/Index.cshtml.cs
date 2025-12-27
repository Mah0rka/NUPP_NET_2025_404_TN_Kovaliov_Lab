using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Fish.Infrastructure;
using Fish.Infrastructure.Models;

namespace Fish.MVC.Pages_Aquariums
{
    public class IndexModel : PageModel
    {
        private readonly Fish.Infrastructure.FishContext _context;

        public IndexModel(Fish.Infrastructure.FishContext context)
        {
            _context = context;
        }

        public IList<AquariumModel> AquariumModel { get;set; } = default!;

        public async Task OnGetAsync()
        {
            AquariumModel = await _context.Aquariums.ToListAsync();
        }
    }
}
