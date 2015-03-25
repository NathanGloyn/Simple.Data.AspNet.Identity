using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Email
{
    [TestFixture]
    public class When_checking_email_confirmed
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
            Assert.Throws<ObjectDisposedException>(async () => await _target.GetEmailConfirmedAsync(new IdentityUser()));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(async () => await _target.GetEmailConfirmedAsync(null));
        }

        [Test]
        public async void Should_return_email_confirmed()
        {
            var emailConfirmed = await _target.GetEmailConfirmedAsync(TestData.GetTestUserJohn());

            Assert.That(emailConfirmed, Is.True);
        }
    }
}