using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Simple.Data.AspNet.Identity
{
    class UserClaimsTable
    {
        private readonly dynamic _db;
        private readonly Tables _tables;

        public UserClaimsTable(dynamic db, Tables tables) 
        {
            _db = db;
            _tables = tables;
        }

        public async Task<ClaimsIdentity> FindByUserId(string userId)
        {
            var claims = new ClaimsIdentity();

            List<IdentityClaim> userClaims = await _db[_tables.UsersClaims].FindAllByUserId(userId);

            foreach (var claim in userClaims)
            {
                claims.AddClaim(claim);
            }

            return claims;
        }

        public async Task AddClaim(IdentityClaim claim)
        {
            await _db[_tables.UsersClaims].Insert(claim);
        }

        public async Task RemoveClaim(IdentityClaim claim)
        {
            await _db[_tables.UsersClaims].Delete(UserId: claim.UserId, ClaimType: claim.ClaimType, ClaimValue: claim.ClaimValue);
        }
    }
}