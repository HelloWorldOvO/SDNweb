using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace SDNweb.Pages
{
    public class RedirectModel : PageModel
    {
        public void OnGet()
        {
           //Response.write("<script>window.open(newpage.aspx;_blank;)</script>");
        }
    }
}
