using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WinterIntex.Pages
{
    public class CompletedModel : PageModel
    {
        [Authorize]
        public void OnGet()
        {
        }
    }
}
