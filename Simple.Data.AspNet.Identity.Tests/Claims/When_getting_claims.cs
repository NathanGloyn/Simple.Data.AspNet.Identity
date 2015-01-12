using System;
using System.Linq;
using System.Security.Claims;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Claims
{
    [TestFixture]
    public class When_getting_claims
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
            Assert.Throws<ArgumentNullException>(() => _target.GetClaimsAsync(null));
        }

        [Test]
        public void Should_return_no_claims_for_user_that_has_none()
        {
            var user = new IdentityUser { Id = TestData.Sue_UserId, UserName = "Sue" };

            var task = _target.GetClaimsAsync(user);

            task.Wait();

            var claims = task.Result;

            Assert.That(claims, Is.Empty);
        }

        [Test]
        public void Should_return_claims_for_user()
        {
            var user = new IdentityUser { Id = TestData.John_UserId, UserName = "John" };

            var task = _target.GetClaimsAsync(user);

            task.Wait();

            var claims = task.Result;

            Assert.That(claims, Is.Not.Empty);
            
        }

        [Test]
        public void Should_return_expected_claims_for_user()
        {
            var user = new IdentityUser { Id = TestData.John_UserId, UserName = "John" };

            var task = _target.GetClaimsAsync(user);

            task.Wait();

            var claims = task.Result;

            Assert.That(claims.Count, Is.EqualTo(2));

            var emailClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var countryClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Country);

            Assert.That(emailClaim, Is.Not.Null);
            Assert.That(emailClaim.Value, Is.EqualTo("John@test.com"));
            Assert.That(countryClaim, Is.Not.Null);
            Assert.That(countryClaim.Value, Is.EqualTo("UK"));
        }
    }
}