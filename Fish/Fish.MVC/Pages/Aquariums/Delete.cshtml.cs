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
    public class DeleteModel : PageModel
    {
        private readonly Fish.Infrastructure.FishContext _context;

        public DeleteModel(Fish.Infrastructure.FishContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AquariumModel AquariumModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aquariummodel = await _context.Aquariums.FirstOrDefaultAsync(m => m.Id == id);

            if (aquariummodel is not null)
            {
                AquariumModel = aquariummodel;

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

            var aquariummodel = await _context.Aquariums.FindAsync(id);
            if (aquariummodel != null)
            {
                AquariumModel = aquariummodel;
                _context.Aquariums.Remove(AquariumModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
