using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Fish.Infrastructure;
using Fish.Infrastructure.Models;

namespace Fish.MVC.Pages_Fishes
{
    public class CreateModel : PageModel
    {
        private readonly Fish.Infrastructure.FishContext _context;

        public CreateModel(Fish.Infrastructure.FishContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AquariumId"] = new SelectList(_context.Aquariums, "Id", "Location");
            return Page();
        }

        [BindProperty]
        public FishModel FishModel { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Fishes.Add(FishModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
