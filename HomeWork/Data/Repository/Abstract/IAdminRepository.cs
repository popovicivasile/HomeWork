using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using HomeWork.Models.Admin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeWork.Data.Repository.Abstract
{
    public interface IAdminRepository
    {
        Task<List<UserRegistration>> GetAllDoctorsAsync();
        Task<UserRegistration> UpdateDoctorAsync(string doctorId, DoctorUpdateDto updateDto);
        Task DeleteDoctorAsync(string doctorId);

   
        Task<List<RefDentalProcedures>> GetAllProceduresAsync();
        Task<RefDentalProcedures> CreateProcedureAsync(ProcedureDto procedureDto);
        Task<RefDentalProcedures> UpdateProcedureAsync(Guid procedureId, ProcedureDto updateDto);
        Task DeleteProcedureAsync(Guid procedureId);
    }
}
