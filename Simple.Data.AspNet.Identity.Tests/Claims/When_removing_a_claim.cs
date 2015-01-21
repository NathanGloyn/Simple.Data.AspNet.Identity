using System;
using System.Security.Claims;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Claims
{
    [TestFixture]
    public class When_removing_a_claim
    {
        private UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
            TestData.AddClaimsToUsers();
            _target = new UserStore<IdentityUser>();
        }

        [Test]
        public void Should_throw_argument_null_exception_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.RemoveClaimAsync(null, new Claim("", "")));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _target.RemoveClaimAsync(new IdentityUser(), new Claim("","")));
        }

        [Test]
        public void Should_throw_argument_null_exception_if_claim_is_null()
        {
            var user = new IdentityUser();

            Assert.Throws<ArgumentNullException>(() => _target.RemoveClaimAsync(user, null));
        }

        [Test]
        public void Should_remove_claim()
        {
            var user = TestData.GetTestUserJohn();
            var claimToDelete = new Claim(ClaimTypes.Email, "John@Test.com");

            var task = _target.RemoveClaimAsync(user, claimToDelete);

            task.Wait();


            var db = Database.Open();
            var claims = db.AspNetUserClaims.FindAllByUserId(TestData.John_UserId).ToList();

            Assert.That(claims.Count, Is.EqualTo(1));

            bool foundClaim=false;
            foreach (var claim in claims)
            {
                if (claim.ClaimType == ClaimTypes.Email)
                {
                    foundClaim = true;
                }   
            }

            Assert.That(foundClaim, Is.False);;
        }
    }
}