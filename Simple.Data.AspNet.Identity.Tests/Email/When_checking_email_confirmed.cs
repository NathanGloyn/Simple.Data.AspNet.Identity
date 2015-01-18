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
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.GetEmailConfirmedAsync(null));
        }

        [Test]
        public void Should_return_email_confirmed()
        {
            var task = _target.GetEmailConfirmedAsync(TestData.GetTestUserJohn());

            task.Wait();

            Assert.That(task.Result, Is.True);
        }
    }
}