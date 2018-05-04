using NoName.Model.Account;
using System.Threading.Tasks;

namespace NoName.Core.Service
{
    public interface IAccountService
    {
        Task<TokenResponseModel> Token(TokenRequestModel model);
    }
}