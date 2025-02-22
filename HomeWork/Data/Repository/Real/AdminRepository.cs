using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using HomeWork.Data.Repository.Abstract;
using HomeWork.Models;
using HomeWork.Models.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork.Data.Repository.Real
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DentalDbContext _dbContext;
        private readonly UserManager<UserRegistration> _userManager;

        public AdminRepository(DentalDbContext dbContext, UserManager<UserRegistration> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<UserRegistration>> GetAllDoctorsAsync()
        {
            List<UserRegistration> result = new List<UserRegistration>();  
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            if (doctors != null)
            {
                foreach (var doctor in doctors)
                {
                    result.Add(doctor);
                }
            }
            return result;
        }

        public async Task<UserRegistration> UpdateDoctorAsync(string doctorId, DoctorUpdateDto updateDto)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            if (doctor == null || !await _userManager.IsInRoleAsync(doctor, "Doctor"))
                throw new Exception("Doctor not found.");

            doctor.FirstName = updateDto.FirstName ?? doctor.FirstName;
            doctor.LastName = updateDto.LastName ?? doctor.LastName;
            doctor.PhoneNumber = updateDto.PhoneNumber ?? doctor.PhoneNumber;

            await _userManager.UpdateAsync(doctor);

            if (updateDto.RefDentalProcedureIds != null)
            {
                var existing = _dbContext.DoctorDentalProcedures.Where(ddp => ddp.UserId == doctorId);
                _dbContext.DoctorDentalProcedures.RemoveRange(existing);
                _dbContext.DoctorDentalProcedures.AddRange(updateDto.RefDentalProcedureIds
                    .Select(pid => new DoctorDentalProcedure { UserId = doctorId, RefDentalProcedureId = pid }));
                await _dbContext.SaveChangesAsync();
            }

            return doctor;
        }

        public async Task DeleteDoctorAsync(string doctorId)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            if (doctor == null || !await _userManager.IsInRoleAsync(doctor, "Doctor"))
                throw new Exception("Doctor not found.");

            await _userManager.DeleteAsync(doctor);
        }

 
        public async Task<List<RefDentalProcedures>> GetAllProceduresAsync()
        {
            return await _dbContext.RefDentalProcedures.ToListAsync();
        }

        public async Task<RefDentalProcedures> CreateProcedureAsync(ProcedureDto procedureDto)
        {
            var procedure = new RefDentalProcedures
            {
                Id = Guid.NewGuid(),
                Name = procedureDto.Name,
                DurationInMinutes = procedureDto.DurationInMinutes
            };
            _dbContext.RefDentalProcedures.Add(procedure);
            await _dbContext.SaveChangesAsync();
            return procedure;
        }

        public async Task<RefDentalProcedures> UpdateProcedureAsync(Guid procedureId, ProcedureDto updateDto)
        {
            var procedure = await _dbContext.RefDentalProcedures.FindAsync(procedureId);
            if (procedure == null)
                throw new Exception("Procedure not found.");

            procedure.Name = updateDto.Name ?? procedure.Name;
            procedure.DurationInMinutes = updateDto.DurationInMinutes != 0 ? updateDto.DurationInMinutes : procedure.DurationInMinutes;
            await _dbContext.SaveChangesAsync();
            return procedure;
        }

        public async Task DeleteProcedureAsync(Guid procedureId)
        {
            var procedure = await _dbContext.RefDentalProcedures.FindAsync(procedureId);
            if (procedure == null)
                throw new Exception("Procedure not found.");

            _dbContext.RefDentalProcedures.Remove(procedure);
            await _dbContext.SaveChangesAsync();
        }
    }
}