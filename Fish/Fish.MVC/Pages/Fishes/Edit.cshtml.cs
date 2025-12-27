using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fish.Infrastructure;
using Fish.Infrastructure.Models;

namespace Fish.MVC.Pages_Fishes
{
    public class EditModel : PageModel
    {
        private readonly Fish.Infrastructure.FishContext _context;

        public EditModel(Fish.Infrastructure.FishContext context)
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

            var fishmodel =  await _context.Fishes.FirstOrDefaultAsync(m => m.Id == id);
            if (fishmodel == null)
            {
                return NotFound();
            }
            FishModel = fishmodel;
           ViewData["AquariumId"] = new SelectList(_context.Aquariums, "Id", "Location");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(FishModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FishModelExists(FishModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FishModelExists(int id)
        {
            return _context.Fishes.Any(e => e.Id == id);
        }
    }
}
