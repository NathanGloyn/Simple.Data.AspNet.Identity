using System;
using Microsoft.AspNet.Identity;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Login
{
    [TestFixture]
    public class When_finding_login
    {
        private UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
            TestData.AdLoginsForUsers();
            _target = new UserStore<IdentityUser>();
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_loginInfo_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.FindAsync(null));
        }

        [Test]
        public void Should_return_null_if_user_not_found()
        {
            var login = new UserLoginInfo("Yahoo", "ghi");
            var task = _target.FindAsync(login);
            task.Wait();

            Assert.That(task.Result, Is.Null);
        }

        [Test]
        public void Should_return_user_for_login()
        {
            var login = new UserLoginInfo("Google", "123");
            var task = _target.FindAsync(login);
            task.Wait();

            Assert.That(task.Result.Id, Is.EqualTo(TestData.John_UserId));

        }
    }
}