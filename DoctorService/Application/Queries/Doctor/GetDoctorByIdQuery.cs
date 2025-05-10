using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Application.Queries.Doctor
{
    public class GetDoctorByIdQuery : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
