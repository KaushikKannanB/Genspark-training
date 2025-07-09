
using System.Threading.Tasks;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FirstAPI.Controllers
{


    [ApiController]
    [Route("/api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IRepository<string, Appointmnet> apprepo;

        public DoctorController(IDoctorService doctorService, IRepository<string, Appointmnet> app)
        {
            _doctorService = doctorService;
            apprepo = app;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<DoctorsBySpecialityResponseDto>>> GetDoctors(string speciality)
        {
            var result = await _doctorService.GetDoctorsBySpeciality(speciality);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor([FromBody] DoctorAddRequestDto doctor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var newDoctor = await _doctorService.AddDoctor(doctor);
                if (newDoctor != null)
                    return Created("", newDoctor);
                return BadRequest("Unable to process request at this moment");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Policy = "ExperiencedDoctorOnly")]
        [HttpDelete("appointment")]
        public async Task<IActionResult> DeleteAppointment(string id)
        {
            var result = await apprepo.Delete(id);
            if (result == null)
                return NotFound("Appointment not found");

            return Ok("Appointment deleted successfully");
        }
    }
}