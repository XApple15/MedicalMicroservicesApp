using Microsoft.AspNetCore.Mvc;
using PacientService.Facade;
using PacientService.Model.DTO;

namespace PacientService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientController : ControllerBase
    {
        private readonly PacientFacadeService _facade;

        public PacientController(PacientFacadeService facade)
        {
            _facade = facade;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var patient = await _facade.GetPatientByIdAsync(id);
            return patient is null ? NotFound() : Ok(patient);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _facade.GetAllPatientsAsync());

        [HttpPost]
        public async Task<IActionResult> Create(CreatePacientDTO dto)
        {
            var id = await _facade.CreatePatientAsync(dto);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePacientDTO dto)
        {
            var updated = await _facade.UpdatePatientAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _facade.DeletePatientAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpGet("searchbymedic")]
        public async Task<IActionResult> Search([FromQuery] Guid DoctorUserId, [FromQuery] string? name)
        {
            var patients = await _facade.GetAllPatientsAsync();
            var filteredPatients = patients
                .Where(p => p.DoctorUserId == DoctorUserId &&
                            (string.IsNullOrWhiteSpace(name) ||
                             (!string.IsNullOrEmpty(p.FullName) && p.FullName.Contains(name, StringComparison.OrdinalIgnoreCase))))
                .ToList();
            return Ok(filteredPatients);
        }

        [HttpGet("searchbyname")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            var patients = await _facade.GetAllPatientsAsync();
            var filteredPatients = patients.Where(p => p.FullName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Ok(filteredPatients);
        }

    }
}
