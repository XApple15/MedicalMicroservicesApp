using DoctorService.Data;
using DoctorService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Application.Commands.Doctor
{
    public class UpdateDoctorCommand : IRequestHandler<UpdateDoctorDTO, bool>
    {
        private readonly DoctorDBContext _context;

        public UpdateDoctorCommand(DoctorDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateDoctorDTO request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

                if (doctor == null)
                {
                    throw new InvalidOperationException($"Doctor with UserId {request.UserId} not found.");
                }

                doctor.FullName = request.FullName;
                doctor.CvPath = request.CvPath;
                doctor.PhotoUrl = request.PhotoUrl;
                doctor.Specialties = request.Specialties;

                if (request.Schedule != null && request.Schedule.Any())
                {
                    var existingScheduleEntries = await _context.ScheduleEntries
                        .Where(se => se.DoctorId == doctor.Id)
                        .ToListAsync(cancellationToken);

                    _context.ScheduleEntries.RemoveRange(existingScheduleEntries);

                    var newScheduleEntries = request.Schedule.Select(entry => new ScheduleEntry
                    {
                        Id = Guid.NewGuid(),
                        Day = (DayOfWeek)entry.Day,
                        StartTime = entry.StartTime,
                        EndTime = entry.EndTime,
                        DoctorId = doctor.Id  
                    }).ToList();

                    Console.WriteLine($"Inserting {newScheduleEntries.Count} schedule entries for Doctor ID: {doctor.UserId}");

                    await _context.ScheduleEntries.AddRangeAsync(newScheduleEntries, cancellationToken);
                }

                try
                {
                    int savedChanges = await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return savedChanges > 0;
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Database update error: {ex.Message}");

                    await transaction.RollbackAsync(cancellationToken);
                    throw new InvalidOperationException(
                        $"Could not update doctor record. Error details: {ex.Message}", ex);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}