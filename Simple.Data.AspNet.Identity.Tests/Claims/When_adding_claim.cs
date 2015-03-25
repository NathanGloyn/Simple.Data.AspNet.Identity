using System;
using System.Collections.Generic;
using System.Linq;
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
            Assert.Throws<ArgumentNullException>(async () => await _target.AddClaimAsync(null, new Claim(ClaimTypes.Email, "")));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.AddClaimAsync(new IdentityUser(), new Claim("","")));
        }

        [Test]
           public void Should_throw_argument_null_exception_if_claim_is_null()
        {
            Assert.Throws<ArgumentNullException>(async () => await _target.AddClaimAsync(new IdentityUser(), null));
        }

        [Test]
        public async void Should_add_claim_for_specified_user()
        {
            var user = TestData.GetTestUserSue();
            var claim = new Claim(ClaimTypes.Email, "Sue@test.com");

            await _target.AddClaimAsync(user, claim);

            var db = Database.Open();

            var claims = await db.AspNetUserClaims.FindAllByUserId(TestData.Sue_UserId).ToList();

            Assert.That(claims.Count, Is.EqualTo(1));

        } 
    }
}