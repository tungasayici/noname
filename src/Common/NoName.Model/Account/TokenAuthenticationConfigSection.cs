namespace NoName.Model.Account
{
    public class TokenAuthenticationConfigSection
    {
        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public int Expiration { get; set; }
    }
}
