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

namespace Fish.MVC.Pages_Aquariums
{
    public class EditModel : PageModel
    {
        private readonly Fish.Infrastructure.FishContext _context;

        public EditModel(Fish.Infrastructure.FishContext context)
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

            var aquariummodel =  await _context.Aquariums.FirstOrDefaultAsync(m => m.Id == id);
            if (aquariummodel == null)
            {
                return NotFound();
            }
            AquariumModel = aquariummodel;
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

            _context.Attach(AquariumModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AquariumModelExists(AquariumModel.Id))
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

        private bool AquariumModelExists(int id)
        {
            return _context.Aquariums.Any(e => e.Id == id);
        }
    }
}
