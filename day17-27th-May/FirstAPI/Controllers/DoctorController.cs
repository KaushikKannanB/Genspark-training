using Microsoft.AspNetCore.Mvc;
using FirstAPI.Models;
namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DoctorController : ControllerBase
    {
        static List<Doctor> doctors = new List<Doctor>();
        [HttpGet("get-doctors")]
        public ActionResult<IEnumerable<Doctor>> GetDoctors()
        {
            return Ok(doctors);
        }

        [HttpPost("post-doctor")]
        public ActionResult<Doctor> PostDoctor([FromBody] Doctor doctor)
        {
            doctors.Add(doctor);
            return Ok(doctor);
        }

        [HttpPut("update-doctor")]
        public ActionResult<Doctor> UpdateDoctor(int id, [FromBody] Doctor upddoc)
        {
            var doc = doctors.FirstOrDefault(d => d.Id == id);
            if (!(doc == null))
            {
                doctors.Remove(doc);
                upddoc.Id = doc.Id;
                doctors.Add(upddoc);
            }
            else
                return BadRequest("Does not exist");


            return Ok(upddoc);
        }

        [HttpDelete("delete-doc")]
        public ActionResult DeleteDoctor(int id)
        {
            var doc = doctors.FirstOrDefault(d => d.Id == id);
            if (!(doc == null))
            {
                doctors.Remove(doc);
            }
            else
                return BadRequest("Does not exist");
            return Ok("Removed");
        }
    }
}
