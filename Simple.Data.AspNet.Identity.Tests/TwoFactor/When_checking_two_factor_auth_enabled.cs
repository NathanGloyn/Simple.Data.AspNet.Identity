using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.TwoFactor
{
    [TestFixture]
    public class When_checking_two_factor_auth_enabled
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
        public void Should_throw_ObjectDisposedException_calling_FindByBName_and_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.GetTwoFactorEnabledAsync(new IdentityUser()));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>( async () => await _target.GetTwoFactorEnabledAsync(null));
        }

        [Test]
        public async void Should_return_two_factor_enabled()
        {
            var enabled = await _target.GetTwoFactorEnabledAsync(TestData.GetTestUserJohn());

            Assert.That(enabled, Is.True);
        }         
    }
}