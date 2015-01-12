using System;
using System.Security.Claims;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Claims
{
    [TestFixture]
    public class When_adding_claim
    {
        private UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
            _target = new UserStore<IdentityUser>();
        }

        [Test]
        public void Should_throw_argument_null_exception_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.AddClaimAsync(null, new Claim(ClaimTypes.Email, "")));
        }

        [Test]
           public void Should_throw_argument_null_exception_if_claim_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.AddClaimAsync(new IdentityUser(), null));
        }


        [Test]
        public void Should_add_claim_for_specified_user()
        {
            var user = new IdentityUser { Id = TestData.Sue_UserId, UserName = "Sue" };
            var claim = new Claim(ClaimTypes.Email, "Sue@test.com");

            var task = _target.AddClaimAsync(user, claim);

            task.Wait();

            var db = Database.Open();

            var claims = db.AspNetUserClaims.FindAllByUserId(TestData.Sue_UserId);

            Assert.That(claims.Count(), Is.EqualTo(1));

        }

         
    }
}