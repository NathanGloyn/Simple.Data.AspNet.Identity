using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Email
{
    [TestFixture]
    public class When_finding_user_by_email
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
            Assert.Throws<ObjectDisposedException>(async () => await _target.FindByEmailAsync(""));
        }

        [Test]
        public async void Should_return_null_for_missing_email([Values("", null)] string value)
        {
            var identityUser = await _target.FindByEmailAsync(value);

            Assert.That(identityUser, Is.Null);
        }

        [Test]
        public async void Should_return_null_for_email_that_does_not_exist_in_db()
        {
            var identityUser = await _target.FindByEmailAsync("bob@test.com");

            Assert.That(identityUser, Is.Null);
        }

        [Test]
        public async void Should_return_user_for_email()
        {
            var identityUser = await _target.FindByEmailAsync(TestData.GetTestUserJohn().Email);

            Assert.That(identityUser.Id, Is.EqualTo(TestData.GetTestUserJohn().Id));
        }
    }
}