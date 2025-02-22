using HomeWork.Data.Repository.Abstract;
using HomeWork.Models;
using HomeWork.Models.Admin;
using HomeWork.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HomeWork.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository, ISecurityRepository securityRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _adminRepository.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        [HttpPut("doctors/{doctorId}")]
        public async Task<IActionResult> UpdateDoctor(string doctorId, [FromBody] DoctorUpdateDto updateDto)
        {
            var doctor = await _adminRepository.UpdateDoctorAsync(doctorId, updateDto);
            return Ok(doctor);
        }

        [HttpDelete("doctors/{doctorId}")]
        public async Task<IActionResult> DeleteDoctor(string doctorId)
        {
            await _adminRepository.DeleteDoctorAsync(doctorId);
            return Ok(new { Message = "Doctor deleted successfully" });
        }

        [HttpGet("procedures")]
        public async Task<IActionResult> GetAllProcedures()
        {
            var procedures = await _adminRepository.GetAllProceduresAsync();
            return Ok(procedures);
        }

        [HttpPost("procedures")]
        public async Task<IActionResult> CreateProcedure([FromBody] ProcedureDto procedureDto)
        {
            var procedure = await _adminRepository.CreateProcedureAsync(procedureDto);
            return Ok(procedure);
        }

        [HttpPut("procedures/{procedureId}")]
        public async Task<IActionResult> UpdateProcedure(Guid procedureId, [FromBody] ProcedureDto updateDto)
        {
            var procedure = await _adminRepository.UpdateProcedureAsync(procedureId, updateDto);
            return Ok(procedure);
        }

        [HttpDelete("procedures/{procedureId}")]
        public async Task<IActionResult> DeleteProcedure(Guid procedureId)
        {
            await _adminRepository.DeleteProcedureAsync(procedureId);
            return Ok(new { Message = "Procedure deleted successfully" });
        }
    }
}