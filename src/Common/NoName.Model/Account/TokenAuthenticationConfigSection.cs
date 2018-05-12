namespace NoName.Model.Account
{
    public class TokenAuthenticationConfigSection
    {
        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public int AccessTokenExpiration { get; set; }

        public int RefreshTokenExpiration { get; set; }
    }
}
