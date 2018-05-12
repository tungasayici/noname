using NoName.Model.Account;
using System.Threading.Tasks;

namespace NoName.Core.Service
{
    public interface IAccountService
    {
        Task<AuthResponseModel> Auth(AuthRequestModel model);
    }
}