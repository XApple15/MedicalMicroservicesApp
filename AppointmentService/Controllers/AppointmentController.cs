using AppointmentService.Model.DTO;
using AppointmentService.Service;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddAppointmentDTO dto)
        => Ok(await _service.CreateAsync(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppointmentDTO dto)
            => Ok(await _service.UpdateAsync(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));

        [HttpGet("search")]
        public async Task<IActionResult> GetByPacientId([FromQuery] Guid? pacientUserId, [FromQuery] Guid? doctorUserId, [FromQuery] string? diagnosis)
        {
            var appointments = await _service.SearchAppointmentsAsync(pacientUserId,doctorUserId,diagnosis);
            return appointments is null ? NotFound() : Ok(appointments);
        }
    }

}
