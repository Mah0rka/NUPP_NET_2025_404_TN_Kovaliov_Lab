using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Fish.Infrastructure;
using Fish.Infrastructure.Models;

namespace Fish.MVC.Pages_Fishes
{
    public class DeleteModel : PageModel
    {
        private readonly Fish.Infrastructure.FishContext _context;

        public DeleteModel(Fish.Infrastructure.FishContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FishModel FishModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fishmodel = await _context.Fishes.FirstOrDefaultAsync(m => m.Id == id);

            if (fishmodel is not null)
            {
                FishModel = fishmodel;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fishmodel = await _context.Fishes.FindAsync(id);
            if (fishmodel != null)
            {
                FishModel = fishmodel;
                _context.Fishes.Remove(FishModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
