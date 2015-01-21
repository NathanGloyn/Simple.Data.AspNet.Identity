using System;
using Microsoft.AspNet.Identity;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Login
{
    [TestFixture]
    public class When_adding_login
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
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _target.AddLoginAsync(new IdentityUser(), new UserLoginInfo("","")));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.AddLoginAsync(null, new UserLoginInfo("", "")));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_loginInfo_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.AddLoginAsync(TestData.GetTestUserSue(), null));
        }

        [Test]
        public void Should_add_login_for_user()
        {
            var loginInfo = new UserLoginInfo("Google", "def");
            dynamic task = _target.AddLoginAsync(TestData.GetTestUserSue(), loginInfo);
            task.Wait();

            Assert.That(task.Result, Is.EqualTo(0));
            var db = Database.Open();
            dynamic login = db.AspNetUserLogins.FindAllByUserId(TestData.Sue_UserId).FirstOrDefault();

            Assert.That(login.LoginProvider,Is.EqualTo("Google"));
            Assert.That(login.ProviderKey, Is.EqualTo("def"));
        }
    }
}