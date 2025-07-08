
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

        public PatientController(IPatientService pService)
        {
            this.pService = pService;
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

    }
}