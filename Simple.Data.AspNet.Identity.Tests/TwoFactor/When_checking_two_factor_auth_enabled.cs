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
            Assert.Throws<ObjectDisposedException>(() => _target.GetTwoFactorEnabledAsync(new IdentityUser()));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.GetTwoFactorEnabledAsync(null));
        }

        [Test]
        public void Should_return_phone_confirmed()
        {
            var task = _target.GetTwoFactorEnabledAsync(TestData.GetTestUserJohn());

            task.Wait();

            Assert.That(task.Result, Is.True);
        }         
    }
}