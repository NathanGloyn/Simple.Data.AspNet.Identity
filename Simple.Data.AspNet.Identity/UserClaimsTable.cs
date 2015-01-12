using System.Collections.Generic;
using System.Security.Claims;

namespace Simple.Data.AspNet.Identity
{
    public class UserClaimsTable
    {
        private readonly dynamic _db;
        private readonly Tables _tables;

        public UserClaimsTable(dynamic db, Tables tables) 
        {
            _db = db;
            _tables = tables;
        }

        public ClaimsIdentity FindByUserId(string userId)
        {
            var claims = new ClaimsIdentity();

            List<IdentityClaim> userClaims = _db[_tables.UsersClaims].FindAllByUserId(userId);

            foreach (var claim in userClaims)
            {
                claims.AddClaim(claim);
            }

            return claims;
        }

        public void AddClaim(IdentityClaim claim)
        {
            _db[_tables.UsersClaims].Insert(claim);
        }

        public void RemoveClaim(IdentityClaim claim)
        {
            _db[_tables.UsersClaims].Delete(UserId: claim.UserId, ClaimType: claim.ClaimType, ClaimValue: claim.ClaimValue);
        }
    }
}