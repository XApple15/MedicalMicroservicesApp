using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Application.Queries.Doctor
{
    public class SearchDoctorQuery : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
