namespace NoName.Model.Account
{
    public class AuthRequestModel
    {
        public string MemberIdentifier { get; set; }

        public string Password { get; set; }

        public string refresh_token { get; set; }

        public string grant_type { get; set; }
    }
}