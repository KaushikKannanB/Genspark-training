using Bank.Interfaces;
using Bank.Misc;
using Bank.Models;
using Bank.Models.DTOs;
using Bank.Repositories;
using FuzzySharp;
using Microsoft.AspNetCore.Http.HttpResults;


namespace Bank.Services
{
    public class FAQServices : IFAQServices
    {
        private readonly IFAQRepository faqrepo;
        private readonly IUserRepository userrepo;

        private readonly ITransactionServices transactionServices;
        private readonly UserInteractionMapper UserInteractionMapper;
        private readonly HttpClient _httpClient;

        public FAQServices(IFAQRepository f, IUserRepository u, HttpClient httpClient, ITransactionServices ts)
        {
            faqrepo = f;
            UserInteractionMapper = new UserInteractionMapper();
            userrepo = u;
            _httpClient = httpClient;
            transactionServices = ts;
        }
        private static readonly List<FAQ> FAQs = new()
        {
            new FAQ { Question = "How do I create a new bank account?", Answer = "You can create an account by clicking the 'Create User' and enter the required credentials and you will be returned with your account number, please remember it for further processes." },
            new FAQ { Question = "How can I see my transactions", Answer = "You can go see all your transactions by entering your Account Number in the 'transactions/id' section, you will be able to see all the information regarding your transactions" },
            new FAQ { Question = "How can I check my account balance?", Answer = "You can check your balance by entering your Account Number in the 'user/id' section, you will be able to see all the information regarding your account including your balance" },
            new FAQ { Question = "How do I transfer money to another account?", Answer = "Go to 'Transfer', enter your account number and password then enter the recipient's account number then enter the amount to be transfered, if you have enough amount, the transfer will be completed smoothly.." },
            new FAQ { Question = "How can I deposit amount?", Answer = "Go to 'Deposit', enter your account number and password, if credentials matched, the specified amount will be deposited smoothly." },
            new FAQ { Question = "How can I withdraw amount?", Answer = "Go to 'Withdraw', enter your account number and password, if credentials matched and if  you have enough balance, the specified amount will be withdrawn smoothly." },
            new FAQ { Question = "How can I make an Interaction?", Answer = "You can either make a simple interaction by asking a FAQ or login by entering your username and password, and get a quick glimpse of your account stats!" }
            
        };

        public async Task<string> GetGeneralQueries(UserSpecificDTO request)
        {

            var user = await userrepo.GetUserById(request.UserId);
            if (user == null)
            {
                return null;
            }
            else
            {
                if (user.Password == request.Password)
                {
                    if (string.IsNullOrWhiteSpace(request.Question)) return "Please enter a valid question.";

                    var bestMatch = FAQs
                        .OrderByDescending(f => Fuzz.Ratio(f.Question.ToLower(), request.Question.ToLower()))
                        .First();

                    FAQ? f = UserInteractionMapper.MapInteraction(request);
                    f.Answer = bestMatch.Answer;

                    int id = await faqrepo.AddInteraction(f);
                    return bestMatch.Answer;
                }
                else
                {

                    return null;
                }
            }

        }
        public async Task<string> GetUserSpecificQueries(UserSpecificDTO request)
        {
            var user = await userrepo.GetUserById(request.UserId);
            if (user == null || user.Password != request.Password)
            {
                return "Invalid user credentials.";
            }

            if (string.IsNullOrWhiteSpace(request.Question))
            {
                return "Please enter a valid question.";
            }

            // Prepare request to your Python ML API
            var apiUrl = "http://127.0.0.1:5000/predict"; // your Flask API url
            var payload = new { question = request.Question };

            try
            {
                // POST the question JSON to the Python model API
                var response = await _httpClient.PostAsJsonAsync(apiUrl, payload);

                if (!response.IsSuccessStatusCode)
                {
                    return "Error contacting intent classification service.";
                }

                // Parse JSON response, expected format: { "intent": "balance_check" }
                var jsonResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

                if (jsonResponse == null || !jsonResponse.ContainsKey("intent"))
                {
                    return "Failed to classify the question intent.";
                }

                string intent = jsonResponse["intent"];
                string answer = "";
                // Call backend methods based on intent
                switch (intent)
                {
                    case "balance_check":
                        answer = await GetBalance(request.UserId);
                        break;

                    case "transaction_count":
                        answer = await GetTransactionCount(request.UserId);
                        break;

                    case "recent_transactions":
                        answer = await GetRecentTransactions(request.UserId);
                        break;

                    case "deposit_count":
                        answer = await GetDepositCount(request.UserId);
                        break;

                    case "withdrawal_count":
                        answer = await GetWithdrawalCount(request.UserId);
                        break;

                    case "total_deposited":
                        answer = await GetTotalDepositedAmount(request.UserId);
                        break;

                    case "total_withdrawn":
                        answer = await GetTotalWithdrawnAmount(request.UserId);
                        break;

                    default:
                        answer = "Sorry, I couldn't understand your request.";
                        break;
                }

                FAQ? f = UserInteractionMapper.MapInteraction(request);
                f.Answer = answer;

                int id = await faqrepo.AddInteraction(f);

                return answer;
            }
            catch (Exception ex)
            {
                // Log ex if needed
                return "Internal error in processing your request.";
            }
        }
        public async Task<string> GetBalance(string id)
        {
            var t = await userrepo.GetUserById(id);
            return $"Your Current Balance is {t.Balance}";
        }
        public async Task<string> GetTransactionCount(string id)
        {
            var t = await userrepo.GetUserById(id);
            return $"The number of transactions you have made since creating your account is {t.Transactions.Count()}";
        }
        public async Task<string> GetRecentTransactions(string id)
        {
            var t = await userrepo.GetUserById(id);
            var tt = t.Transactions.OrderByDescending(t => t.Date).Take(5).ToList();
            string res = "";
            foreach (var i in tt)
            {
                res += $"TransactionId:{i.Id}, Type:{i.Type}, Amount Involved:{i.Amount}";
                res += "'/n'";
            }
            return res;
        }
        public async Task<string> GetDepositCount(string id)
        {
            var t = await userrepo.GetUserById(id);
            return $"The number of Deposits(neglecting The Initila deposit) that you have made since creating your account is {t.Transactions.Where(t => t.Type == "Deposit").Count()}";
        }
        public async Task<string> GetWithdrawalCount(string id)
        {
            var t = await userrepo.GetUserById(id);
            return $"The number of Withdrawals that you have made since creating your account is {t.Transactions.Where(t => t.Type == "Withdraw").Count()}";
        }
        public async Task<string> GetTotalDepositedAmount(string id)
        {
            var t = await userrepo.GetUserById(id);
            float tt = t.Transactions.Where(t => t.Type == "Deposit" || t.Type == "Initial Deposit").Sum(t => t.Amount);
            return $"The Total Amount you have deposited to your account since creation is {tt}";
        }
        public async Task<string> GetTotalWithdrawnAmount(string id)
        {
            var t = await userrepo.GetUserById(id);
            float tt = t.Transactions.Where(t => t.Type == "Withdraw").Sum(t => t.Amount);
            return $"The Total Amount you have withdrawn from your account since creation is {tt}";
        }
    }
}