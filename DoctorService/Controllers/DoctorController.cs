using DoctorService.Application.Commands.Doctor;
using DoctorService.Application.Queries.Doctor;
using DoctorService.Application.Service;
using DoctorService.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Controllers
{
   // the ID i am searching is the USERID !!!!
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;
        private readonly DoctorDBContext _dbContext;
        public DoctorController(IMediator mediator, IFileService fileService,DoctorDBContext dbContext)
        {
            _mediator = mediator;
            _fileService = fileService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _mediator.Send(new GetAllDoctorsQuery());
            return Ok(doctors);
        }

        [HttpGet("groupbyspeciality")]
        public async Task<IActionResult> GetAllGroupedBySpeciality()
        {
            var doctors = await _mediator.Send(new GetAllDoctorsGroupedBySpecialityQuery());
            return Ok(doctors);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddDoctorDTO command)
        {
            var doctorId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDoctorByUserId), new { userId = doctorId }, new { userId = doctorId });
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


        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] string? specialty)
        {
            var result = await _mediator.Send(new SearchDoctorQueryDTO
            {
                Name = name,
                Specialty = specialty
            });

            return Ok(result);
        }


        [HttpPost("{userId}/profile-image")]
        public async Task<IActionResult> UploadProfileImage(Guid userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedImageExtensions.Contains(fileExtension))
                return BadRequest("Invalid file type. Only jpg, jpeg, png, and gif are allowed.");

            try
            {
                var doctorEntity = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
                if (doctorEntity == null)
                    return NotFound();

                string fileUrl = await _fileService.SaveFileAsync(file, "images/doctors");

                doctorEntity.PhotoUrl = fileUrl;
                await _dbContext.SaveChangesAsync();

                return Ok(new { ImageUrl = fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("{userId}/cv")]
        public async Task<IActionResult> UploadCv(Guid userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            string[] allowedDocExtensions = { ".pdf", ".doc", ".docx" };
            string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedDocExtensions.Contains(fileExtension))
                return BadRequest("Invalid file type. Only pdf, doc, and docx are allowed for CV files.");

            try
            {
                var doctorEntity = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
                if (doctorEntity == null)
                    return NotFound();

                string filePath = await _fileService.SaveFileAsync(file, "documents/cvs");
                doctorEntity.CvPath = filePath;

                await _dbContext.SaveChangesAsync();

                return Ok(new { CvFilePath = filePath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{userId}/profile-image")]
        public async Task<IActionResult> GetProfileImage(Guid userId)
        {
            var doctorQuery = new GetDoctorByUserIdQuery(userId);

            var doctor = await _mediator.Send(doctorQuery);
            if (doctor == null || string.IsNullOrEmpty(doctor.PhotoUrl))
                return NotFound();

            try
            {
                var fileStream = await _fileService.GetFileStreamAsync(doctor.PhotoUrl);
                string mimeType = GetMimeTypeFromPath(doctor.PhotoUrl);
                return File(fileStream, mimeType);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{userId}/cv")]
        public async Task<IActionResult> GetCv(Guid userId)
        {
            var doctorQuery = new GetDoctorByUserIdQuery(userId);

            var doctor = await _mediator.Send(doctorQuery); if (doctor == null || string.IsNullOrEmpty(doctor.CvPath))
                return NotFound();

            try
            {
                var fileStream = await _fileService.GetFileStreamAsync(doctor.CvPath);
                string mimeType = GetMimeTypeFromPath(doctor.CvPath);
                string fileName = Path.GetFileName(doctor.CvPath);
                return File(fileStream, mimeType, fileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        private string GetMimeTypeFromPath(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }

    }
}
