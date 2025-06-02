using Bank.Models;
using Bank.Models.DTOs;
namespace Bank.Misc
{
    public class UserInteractionMapper
    {
        public FAQ? MapInteraction(UserSpecificDTO request)
        {
            FAQ f = new();
            f.UserId = request.UserId;
            f.Question = request.Question;

            return f;
        }
    }
}