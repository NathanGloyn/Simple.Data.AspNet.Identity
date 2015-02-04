using System.Security.Claims;

namespace Simple.Data.AspNet.Identity
{
    class IdentityClaim
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

        /// <summary>
        /// Provides an implicit conversion from our custom class to a 
        /// standard <see cref="Claim"/> class
        /// </summary>
        /// <param name="identityClaim">Instance of an IdentityClaim</param>
        /// <returns>A <see cref="Claim"/> instance.</returns>
        public static implicit operator Claim(IdentityClaim identityClaim)
        {
            return new Claim(identityClaim.ClaimType,identityClaim.ClaimValue);
        }
    }
}