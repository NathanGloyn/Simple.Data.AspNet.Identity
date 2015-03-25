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
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.FindAsync(new UserLoginInfo("", "")));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_loginInfo_is_null()
        {
            Assert.Throws<ArgumentNullException>(async () => await _target.FindAsync(null));
        }

        [Test]
        public async  void Should_return_null_if_user_not_found()
        {
            var login = new UserLoginInfo("Yahoo", "ghi");
            var userLogin = await _target.FindAsync(login);

            Assert.That(userLogin, Is.Null);
        }

        [Test]
        public async void Should_return_user_for_login()
        {
            var login = new UserLoginInfo("Google", "123");
            var userLogin = await _target.FindAsync(login);

            Assert.That(userLogin.Id, Is.EqualTo(TestData.John_UserId));

        }
    }
}