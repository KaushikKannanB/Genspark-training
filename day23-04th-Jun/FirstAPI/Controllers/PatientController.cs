
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
    public class PatientController : ControllerBase
    {
        private readonly IPatientService pService;
        private readonly IAppointmentService appservice;

        public PatientController(IPatientService pService, IAppointmentService a)
        {
            this.pService = pService;
            appservice = a;
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient([FromBody] PatientAddRequest pat)
        {
            try
            {
                var newp = await pService.AddPatient(pat);
                if (newp != null)
                    return Created("", newp);
                return BadRequest("Unable to process request at this moment");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("add-appointment")]
        [Authorize(Roles ="Patient")]
        public async Task<ActionResult<Patient>> AddAppointment([FromBody] AppointAddRequest request)
        {
            var newa = await appservice.AddAppointment(request);
            if (newa == null)
            {
                return BadRequest("Failed booking appointment");
            }
            else
            {
                return Ok(newa);
            }

        }

    }
}