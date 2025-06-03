using System.Security.Claims;
using FirstAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Authorization
{


    public class ExperienceHandler : AuthorizationHandler<ExperienceRequirement>
    {
        private readonly IDoctorService _doctorService;

        public ExperienceHandler(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ExperienceRequirement requirement)
        {
            var name = context.User.Identity?.Name;

            if (string.IsNullOrEmpty(name)) return;

            var doctor = await _doctorService.GetDoctByMail(name);
            if (doctor != null && doctor.YearsOfExperience > requirement.MinimumYears)
            {
                context.Succeed(requirement);
            }
        }
    }
}
