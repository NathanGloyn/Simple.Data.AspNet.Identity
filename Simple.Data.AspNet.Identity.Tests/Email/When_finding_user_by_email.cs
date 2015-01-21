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
            Assert.Throws<ObjectDisposedException>(() => _target.FindByEmailAsync(""));
        }

        [Test]
        public void Should_return_null_for_missing_email([Values("", null)] string value)
        {
            var task = _target.FindByEmailAsync(value);

            task.Wait();

            Assert.That(task.Result, Is.Null);
        }

        [Test]
        public void Should_return_null_for_email_that_does_not_exist_in_db()
        {
            var task = _target.FindByEmailAsync("bob@test.com");

            task.Wait();

            Assert.That(task.Result, Is.Null);
        }

        [Test]
        public void Should_return_user_for_email()
        {
            var task = _target.FindByEmailAsync(TestData.GetTestUserJohn().Email);

            task.Wait();

            Assert.That(task.Result.Id, Is.EqualTo(TestData.GetTestUserJohn().Id));
        }
    }
}