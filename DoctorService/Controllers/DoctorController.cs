using DoctorService.Application.Commands.Doctor;
using DoctorService.Application.Queries.Doctor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Controllers
{
   // the ID i am searching is the USERID !!!!
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DoctorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _mediator.Send(new GetAllDoctorsQuery());
            return Ok(doctors);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddDoctorDTO command)
        {
            var doctorId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDoctorByUserId), new { userId = doctorId }, null);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetDoctorByUserId(Guid userId)
        {
            var doctorQuery = new GetDoctorByUserIdQuery(userId);

            var doctor = await _mediator.Send(doctorQuery);

            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            return Ok(doctor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteDoctorDTO(id));
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDoctorDTO command)
        {
            if (id != command.UserId)
                return BadRequest("Doctor ID in URL does not match body");

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent(); 
        }

        
    }
}
