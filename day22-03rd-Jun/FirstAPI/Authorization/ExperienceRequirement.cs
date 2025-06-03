using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Authorization
{


    public class ExperienceRequirement : IAuthorizationRequirement
    {
        public int MinimumYears { get; }

        public ExperienceRequirement(int years)
        {
            MinimumYears = years;
        }
    }
}