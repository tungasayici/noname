using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NoName.Common.Foundation;
using NoName.Core.Exception;
using NoName.Core.Service;
using NoName.Localization;
using NoName.Model.Account;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Service
{
    public class AccountService : IAccountService
    {
        private readonly TokenAuthenticationConfigSection _tokenAuthenticationModel;

        public AccountService(IOptions<TokenAuthenticationConfigSection> tokenAuthenticationModel)
        {
            _tokenAuthenticationModel = tokenAuthenticationModel.Value;
        }

        public async Task<TokenResponseModel> Token(TokenRequestModel model)
        {
            if (String.Equals(model.grant_type, Constants.GrantType.Password, StringComparison.OrdinalIgnoreCase))
            {
                //if (String.IsNullOrEmpty(model.Email))
                //{
                //    throw new ModelValidationException(nameof(model.PhoneNumber), AccountLocalization.NeedPhoneNumber);
                //}

                //if (String.IsNullOrEmpty(model.Password))
                //{
                //    throw new ModelValidationException(nameof(model.Password), AccountLocalization.NeedPassword);
                //}

                //var member = await FindMember(model.PhoneNumber, model.Password);
                //if (member == null)
                //{
                //    throw new BusinessException(Constants.Exception.MEMBER_NOT_FOUND, AccountLocalization.MemberNotFound);
                //}

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenAuthenticationModel.SecretKey));

                var claims = new Claim[]
                {
                    new Claim(Constants.Claim.UserId, "123456789"),
                    new Claim(ClaimTypes.Role, "SuperAdmin"),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var now = DateTime.UtcNow;
                var expiration = TimeSpan.FromMinutes(_tokenAuthenticationModel.Expiration);
                var jwt = new JwtSecurityToken(
                    issuer: _tokenAuthenticationModel.Issuer,
                    audience: _tokenAuthenticationModel.Issuer,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(expiration),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return new TokenResponseModel { access_token = encodedJwt, expires_in = (int)expiration.TotalSeconds };
            }
            else if (String.Equals(model.grant_type, Constants.GrantType.Refresh_Token, StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException(HttpStatusCode.NotImplemented, Constants.Exception.INVALID_GRANT_TYPE, AccountLocalization.InvalidGrantType);
            }
            else
            {
                throw new BusinessException(Constants.Exception.INVALID_GRANT_TYPE, AccountLocalization.InvalidGrantType);
            }
        }
    }
}