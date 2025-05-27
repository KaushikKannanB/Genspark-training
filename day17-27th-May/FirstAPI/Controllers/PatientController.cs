using Microsoft.AspNetCore.Mvc;
using FirstAPI.Models;
namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]

    public class PatientController : ControllerBase
    {
        static List<Patient> patients = new List<Patient>();

        [HttpGet("getall-patients")]

        public ActionResult<IEnumerable<Patient>> GetAllPatients()
        {
            return Ok(patients);
        }

        [HttpGet("getpatient-ID")]
        public ActionResult<Patient> GetPatientById(int id)
        {
            var pat = patients.FirstOrDefault(p => p.Id == id);
            if (pat == null)
                return BadRequest("Does not exist!!");

            return Ok(pat);
        }

        [HttpGet("getpatient-by-age")]
        public ActionResult<IEnumerable<Patient>> GetByAge(int minage, int maxage)
        {
            var pats = patients.Where(p => p.age >= minage && p.age < maxage);
            if (pats == null)
            {
                return BadRequest("No such patients in this age range");
            }
            else
            {
                return Ok(pats);
            }
        }

        [HttpPost("post-patient")]
        public ActionResult PostPatients([FromBody] Patient pat)
        {
            patients.Add(pat);
            return Ok("Added the patient");
        }


        [HttpPut("update-patient")]
        public ActionResult UpdatePatient(int id, Patient updpat)
        {
            var pat = patients.FirstOrDefault(p => p.Id == id);
            if (pat == null)
            {
                return BadRequest("Nope- does not exist");
            }
            else
            {
                patients.Remove(pat);
                updpat.Id = pat.Id;
                patients.Add(updpat);

                return Ok("Updated");
            }
        }

        [HttpDelete("delete-patient")]

        public ActionResult DeletePatient(int id)
        {
            var pat = patients.FirstOrDefault(p => p.Id == id);
            if (pat == null)
            {
                return BadRequest("Nope- does not exist");
            }
            else
            {
                patients.Remove(pat);
                

                return Ok("Deleted");
            }
        }
    }
}