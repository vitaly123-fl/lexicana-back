using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace lexicana.Authorization
{
    public class AuthOptions
    {
        public const string Jwt = "Jwt";
        
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            if (string.IsNullOrEmpty(Key))
            {
                throw new ArgumentNullException(nameof(Key), "JWT Key cannot be null or empty.");
            }
            
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }
    }
}
