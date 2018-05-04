namespace NoName.Model.Account
{
    public class TokenRequestModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string grant_type { get; set; }
    }
}