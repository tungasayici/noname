using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NoName.Common.Foundation;
using NoName.Core.Exception;
using NoName.Core.Repository;
using NoName.Core.Service;
using NoName.Data.Entity;
using NoName.Enum;
using NoName.Localization;
using NoName.Model.Account;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Service
{
    public class AccountService : IAccountService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TokenAuthenticationConfigSection _tokenAuthenticationModel;

        public AccountService(IMemberRepository memberRepository, IRefreshTokenRepository refreshTokenRepository, IOptions<TokenAuthenticationConfigSection> tokenAuthenticationModel)
        {
            _memberRepository = memberRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenAuthenticationModel = tokenAuthenticationModel.Value;
        }

        public async Task<AuthResponseModel> Auth(AuthRequestModel model)
        {
            if (String.Equals(model.grant_type, Constants.GrantType.Password, StringComparison.OrdinalIgnoreCase))
            {
                if (String.IsNullOrEmpty(model.MemberIdentifier))
                {
                    throw new ModelValidationException(nameof(model.MemberIdentifier), AccountLocalization.NeedMemberIdentifier);
                }

                if (String.IsNullOrEmpty(model.Password))
                {
                    throw new ModelValidationException(nameof(model.Password), AccountLocalization.NeedPassword);
                }

                var member = await _memberRepository.FirstOrDefaultAsync(f => f.MemberIdentifier == model.MemberIdentifier && f.Password == model.Password);
                if (member == null)
                {
                    throw new BusinessException(Constants.Exception.MEMBER_NOT_FOUND, AccountLocalization.MemberNotFound);
                }

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenAuthenticationModel.SecretKey));

                var claims = new Claim[]
                {
                    new Claim(Constants.Claim.UserId, member.Id.ToString()),
                    new Claim(ClaimTypes.Role, "SuperAdmin"),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var accessTokenNow = DateTime.UtcNow;
                var accessTokenExpiration = TimeSpan.FromMinutes(_tokenAuthenticationModel.AccessTokenExpiration);

                var accessToken = new JwtSecurityToken(
                    issuer: _tokenAuthenticationModel.Issuer,
                    audience: _tokenAuthenticationModel.Issuer,
                    claims: claims,
                    notBefore: accessTokenNow,
                    expires: accessTokenNow.Add(accessTokenExpiration),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
                
                var encodedAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);

                var refreshToken = GetRefreshToken();

                var refreshTokenEntity = await _refreshTokenRepository.FirstOrDefaultAsync(f => f.MemberId == member.Id && f.Channel == ChannelEnum.Web);
                if (refreshTokenEntity == null)
                {
                    refreshTokenEntity = new RefreshToken();
                    refreshTokenEntity.Channel = ChannelEnum.Web;
                    refreshTokenEntity.MemberId = member.Id;
                    refreshTokenEntity.RefreshTokenInfo = refreshToken;
                    refreshTokenEntity.RefreshTokenExpireDate = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_tokenAuthenticationModel.RefreshTokenExpiration));

                    await _refreshTokenRepository.AddAndSaveChangesAsync(refreshTokenEntity);
                }
                else
                {
                    refreshTokenEntity.RefreshTokenInfo = refreshToken;
                    refreshTokenEntity.RefreshTokenExpireDate = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_tokenAuthenticationModel.RefreshTokenExpiration));

                    await _refreshTokenRepository.UpdateAndSaveChangesAsync(refreshTokenEntity);
                }

                return new AuthResponseModel { access_token = encodedAccessToken, refresh_token = refreshToken, expires_in = (int)accessTokenExpiration.TotalSeconds };
            }
            else if (String.Equals(model.grant_type, Constants.GrantType.Refresh_Token, StringComparison.OrdinalIgnoreCase))
            {
                if (String.IsNullOrEmpty(model.refresh_token))
                {
                    throw new ModelValidationException(nameof(model.refresh_token), AccountLocalization.NeedRefreshToken);
                }

                var refreshTokenEntity = await _refreshTokenRepository.FirstOrDefaultAsync(f => f.RefreshTokenInfo == model.refresh_token && f.Channel == ChannelEnum.Web);
                if(refreshTokenEntity == null)
                {
                    throw new BusinessException(Constants.Exception.REFRESH_TOKEN_NOT_FOUND, AccountLocalization.RefreshTokenNotFound);
                }

                if(refreshTokenEntity.RefreshTokenExpireDate != null && refreshTokenEntity.RefreshTokenExpireDate < DateTime.UtcNow)
                {
                    throw new BusinessException(Constants.Exception.REFRESH_TOKEN_EXPIRED, AccountLocalization.RefreshTokenExpired);
                }

                var member = await _memberRepository.FirstOrDefaultAsync(f => f.Id == refreshTokenEntity.MemberId);
                if (member == null)
                {
                    throw new BusinessException(Constants.Exception.MEMBER_NOT_FOUND, AccountLocalization.MemberNotFound);
                }

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenAuthenticationModel.SecretKey));

                var claims = new Claim[]
                {
                    new Claim(Constants.Claim.UserId, member.Id.ToString()),
                    new Claim(ClaimTypes.Role, "SuperAdmin"),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var accessTokenNow = DateTime.UtcNow;
                var accessTokenExpiration = TimeSpan.FromMinutes(_tokenAuthenticationModel.AccessTokenExpiration);

                var accessToken = new JwtSecurityToken(
                    issuer: _tokenAuthenticationModel.Issuer,
                    audience: _tokenAuthenticationModel.Issuer,
                    claims: claims,
                    notBefore: accessTokenNow,
                    expires: accessTokenNow.Add(accessTokenExpiration),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

                var encodedAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);

                return new AuthResponseModel { access_token = encodedAccessToken, expires_in = (int)accessTokenExpiration.TotalSeconds };
            }
            else
            {
                throw new BusinessException(Constants.Exception.INVALID_GRANT_TYPE, AccountLocalization.InvalidGrantType);
            }
        }

        private string GetRefreshToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}