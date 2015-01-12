using System.Security.Claims;

namespace Simple.Data.AspNet.Identity
{
    public class IdentityClaim
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public IdentityClaim() { }

        public IdentityClaim(string userId, Claim claim)
        {
            UserId = userId;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        public static implicit operator Claim(IdentityClaim identityClaim)
        {
            return new Claim(identityClaim.ClaimType,identityClaim.ClaimValue);
        }
    }
}